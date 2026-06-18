'*************************************************************************************

'Note:This is the new file 
'************************************************************************************
Option Strict On
Option Explicit On

Imports SiriusFS.SAM.Structure.STSErrorPublisher
Imports SiriusFS.SAM.Structure
Imports System.IO
Imports Microsoft.ApplicationBlocks.ExceptionManagement
Imports Microsoft.ApplicationBlocks.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports SiriusFS.SAM.Structure.BaseImplementationTypes
Imports SiriusFS.SAM.Structure.SAMConstants
Imports dPMDAOBridge
Imports SSP.Shared.gPMConstants
Imports SSP.Shared

Partial Public Class CoreSAMBusiness

    ''' <summary>
    ''' In this function, automatic allocation will  be done
    '''</summary>
    '''<param name="oBusiness" >This is an object of a class CoreBusiness </param>   
    '''<param name="conPayClaim"> This is an object of a class SiriusConnection </param>   
    '''<param name="OAllocation" >This is an object of a class BaseAllocationType</param>   
    '''<param name="oSAMErrorCollection" >This is an object of a class SAMErrorCollection</param>  
    '''<remarks></remarks>

    Private Sub ProcessAllocation(ByVal oBusiness As CoreBusiness, ByRef conPayClaim As SiriusConnection,
    ByVal oAllocation As BaseAllocationType, ByRef oSAMErrorCollection As SAMErrorCollection)

        'If the allocation object is empty then error and exit.
        If oAllocation Is Nothing Then
            oSAMErrorCollection.AddInvalidData(
            SAMConstants.SAMInvalidData.MandatoryInputMissing,
            SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
            "Allocation")
        End If
        oSAMErrorCollection.CheckForErrors()

        'Validate the CashList by calling ValidateAllocation method and deal with any errors 
        'by calling sAMErrorCollection.CheckForErrors
        'ValidateAllocation(oBusiness, conPayClaim, oAllocation, oSAMErrorCollection)
        oSAMErrorCollection.CheckForErrors()

        'For now this method will only handle creating a new allocation so if a non-zero 
        'allocation.AllocationKey has been supplied then raise an error and exit
        If (oAllocation.AllocationKey <> 0) Then
            oSAMErrorCollection.AddInvalidData(SAMConstants.
                       SAMInvalidData.InValidAllocationKeySuppliedForNewAllocation,
                       "For New Allocation ,AllocationKey Should be zero", "Allocation")
        End If
        oSAMErrorCollection.CheckForErrors()

        'For now we shall only be handling automatic allocations so if a manual allocation 
        'has been supplied (allocation.AutoAllocate = false ) then raise an error and exit. 
        If (oAllocation.AutoAllocate = False) Then
            oSAMErrorCollection.AddBusinessRule(SAMConstants.
            SAMBusinessErrors.ManualAllocationNotPossibleInAutoAllocation,
            "While doing automatic allocation, manual allocation is not allowed",
            "Allocation")
        End If
        oSAMErrorCollection.CheckForErrors()

        Try
            conPayClaim.BeginTransaction()
            If (oAllocation.AutoAllocate = True) Then

                AutoAllocateItems(conPayClaim, oAllocation)

            End If

            conPayClaim.CommitTransaction()

        Catch ex As Exception
            conPayClaim.RollbackTransaction()
            Throw
        End Try

    End Sub

    ''' <summary>
    ''' In this function, automatic allocation will  be done
    '''</summary>
    '''<param name="oBusiness" >This is an object of a class CoreBusiness </param>   
    '''<param name="conPayClaim"> This is an object of a class SiriusConnection </param>   
    '''<param name="oAllocation" >This is an object of a class BaseAllocationType</param>   
    '''<param name="oSAMErrorCollection" >This is an object of a class SAMErrorCollection</param>  
    '''<param name="crAllocatedAmount" ></param>  
    '''<remarks></remarks>

    Private Sub ProcessAllocation(ByVal oBusiness As CoreBusiness, ByRef conPayClaim As SiriusConnection,
    ByVal oAllocation As BaseAllocationType, ByRef oSAMErrorCollection As SAMErrorCollection, ByRef crAllocatedAmount As Decimal)

        'If the allocation object is empty then error and exit.
        If oAllocation Is Nothing Then
            oSAMErrorCollection.AddInvalidData(
            SAMConstants.SAMInvalidData.MandatoryInputMissing,
            SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
            "Allocation")
        End If
        oSAMErrorCollection.CheckForErrors()

        'Validate the CashList by calling ValidateAllocation method and deal with any errors 
        'by calling sAMErrorCollection.CheckForErrors
        'ValidateAllocation(oBusiness, conPayClaim, oAllocation, oSAMErrorCollection)
        oSAMErrorCollection.CheckForErrors()

        'For now this method will only handle creating a new allocation so if a non-zero 
        'allocation.AllocationKey has been supplied then raise an error and exit
        If (oAllocation.AllocationKey <> 0) Then
            oSAMErrorCollection.AddInvalidData(SAMConstants.
                       SAMInvalidData.InValidAllocationKeySuppliedForNewAllocation,
                       "For New Allocation ,AllocationKey Should be zero", "Allocation")
        End If
        oSAMErrorCollection.CheckForErrors()

        'For now we shall only be handling automatic allocations so if a manual allocation 
        'has been supplied (allocation.AutoAllocate = false ) then raise an error and exit. 
        If (oAllocation.AutoAllocate = False) Then
            oSAMErrorCollection.AddBusinessRule(SAMConstants.
            SAMBusinessErrors.ManualAllocationNotPossibleInAutoAllocation,
            "While doing automatic allocation, manual allocation is not allowed",
            "Allocation")
        End If
        oSAMErrorCollection.CheckForErrors()

        Try
            conPayClaim.BeginTransaction()
            If (oAllocation.AutoAllocate = True) Then
                AutoAllocateItems(conPayClaim, oAllocation)
                crAllocatedAmount = oAllocation.LeadAllocatingTrans.Amount
            End If

            conPayClaim.CommitTransaction()

        Catch ex As Exception
            conPayClaim.RollbackTransaction()
            Throw
        End Try

    End Sub

    ''' <summary>
    ''' This function will check whether allocation object is empty or not , if it is not empty then it will call
    ''' the function "ValidateBaseAllocationType"
    '''</summary>
    '''<param name="oBusiness" >This is an object of a class CoreBusiness </param>   
    '''<param name="conPayClaim"> This is an object of a class SiriusConnection </param>   
    '''<param name="OAllocation" >This is an object of a class BaseAllocationType</param>   
    '''<param name="oSAMErrorCollection" >This is an object of a class SAMErrorCollection</param>  
    '''<remarks></remarks>

    Private Sub ValidateAllocation(ByVal oBusiness As CoreBusiness, ByVal conPayClaim As SiriusConnection,
    ByVal OAllocation As BaseAllocationType, ByRef oSAMErrorCollection As SAMErrorCollection)

        'If the allocation object is empty then error and exit.
        If OAllocation Is Nothing Then
            oSAMErrorCollection.AddInvalidData(
            SAMConstants.SAMInvalidData.MandatoryInputMissing,
            SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
            "Allocation")
        End If
        oSAMErrorCollection.CheckForErrors()

        'Call ValidateBaseAllocationType method and deal with any errors by calling
        'SAMErrorCollection.CheckForErrors()
        ValidateBaseAllocationType(oBusiness, conPayClaim, OAllocation, oSAMErrorCollection)
        oSAMErrorCollection.CheckForErrors()

    End Sub

    ''' <summary>
    ''' This function will validate the properties of the class "BaseAllocationType"
    '''</summary>
    '''<param name="oBusiness" >This is an object of a class CoreBusiness </param>   
    '''<param name="conPayClaim"> This is an object of a classSiriusConnection </param>   
    '''<param name="OAllocation" >This is an object of a class BaseAllocationType</param>   
    '''<param name="oSAMErrorCollection" >This is an object of a class SAMErrorCollection</param>  
    '''<remarks></remarks>
    Private Sub ValidateBaseAllocationType(ByVal oBusiness As CoreBusiness, ByVal conPayClaim _
    As SiriusConnection,
    ByRef OAllocation As BaseAllocationType, ByRef oSAMErrorCollection As SAMErrorCollection)
        OAllocation.Validate(CObj(oSAMErrorCollection))
        oSAMErrorCollection.CheckForErrors()
    End Sub

    ''' <summary>
    ''' First this function will fetch the other  allocaiton transaction details and with these data
    ''' the auto allocation will be done through the COM
    '''</summary>
    '''<param name="conPayClaim" >This is an object of a class SiriusConnection </param>   
    '''<param name="OAllocation">This is an object of a class BaseAllocationType</param>  
    '''<remarks></remarks>
    Private Sub AutoAllocateItems(ByVal conPayClaim As SiriusConnection,
    ByRef OAllocation As BaseAllocationType)
        Dim oSAMErrorCollection As New SAMErrorCollection
        'If the allocation object is empty then error and exit.
        If OAllocation Is Nothing Then
            oSAMErrorCollection.AddInvalidData(
            SAMConstants.SAMInvalidData.MandatoryInputMissing,
            SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
            "Allocation")
        End If

        Dim bProceedWithAutoAllocation As Boolean = False
        Dim crWriteOffAmount As Decimal
        Dim crCurrencyGainLossAutoAllocaitonAmount As Decimal
        If (OAllocation.OtherAllocatingTrans Is Nothing) Then
            'we are coming from Cash/Cheque Payment, with AutoAllocate = True
            CheckWriteOffAndExchangeRateGainLoss(conPayClaim,
                                                 OAllocation,
                                                 bProceedWithAutoAllocation,
                                                 crWriteOffAmount,
                                                 crCurrencyGainLossAutoAllocaitonAmount)
            If bProceedWithAutoAllocation = False Then
                oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.FailedToAutoAllocateTransactions,
                "Payment transaction has not been auto-allocated.")
                oSAMErrorCollection.CheckForErrors()
            End If
        End If

        'Validate the CashList by calling ValidateAllocation method and deal with any errors 
        'by calling sAMErrorCollection.CheckForErrors
        ValidateAllocation(Nothing, conPayClaim, OAllocation, oSAMErrorCollection)
        oSAMErrorCollection.CheckForErrors()


        oSAMErrorCollection.CheckForErrors()
        If (OAllocation.OtherAllocatingTrans Is Nothing OrElse OAllocation.OtherAllocatingTrans.Length = 0) Then
            GetTransForAllocationForAccount(conPayClaim, OAllocation)
        End If
        Dim r_oOtherAllocationTrans(,) As Object
        ReDim r_oOtherAllocationTrans(2, OAllocation.OtherAllocatingTrans.Length - 1)
        For Cnt As Integer = 0 To OAllocation.OtherAllocatingTrans.Length - 1
            r_oOtherAllocationTrans(0, Cnt) = OAllocation.OtherAllocatingTrans(Cnt).TransDetailKey
            r_oOtherAllocationTrans(1, Cnt) = OAllocation.OtherAllocatingTrans(Cnt).Amount
            r_oOtherAllocationTrans(2, Cnt) = OAllocation.OtherAllocatingTrans(Cnt).AmountCurrencyId
        Next

        'This variable is used to get the result and it will make sure that whether the method is 
        'executed without error or not
        Dim lComReturnValue As Integer

        Dim obACTAllocate As New bACTAllocate.Business
        Dim lReturn As Integer = 0
        'This portion will call the com method to initialize
        'Rk modifies as part of SAM SFI Interop conversions
        Dim oDatabase As Object = Nothing
        If Not conPayClaim Is Nothing Then
            oDatabase = Nothing
            oDatabase = conPayClaim.PMDAODatabase
        End If

        lReturn = CInt(obACTAllocate.Initialise(
                              _SiriusUser.Username,
                              _SiriusUser.Password,
                              _SiriusUser.UserID,
                              _SiriusUser.SourceID,
                              _SiriusUser.LanguageID,
                              _SiriusUser.CurrencyID,
                              1,
                              SiriusUserDefaults.AppName,
                              vDatabase:=oDatabase))

        If (lReturn <> PMEReturnCode.PMTrue) Then

            ' if the account processing fails then throw a business rule error
            oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed,
                                                SAMBusinessErrors.COMComponentMethodFailed.ToString,
                                                "obACTAllocate.Initialise")
            oSAMErrorCollection.CheckForErrors()

        End If
        If bProceedWithAutoAllocation Then
            Dim nWriteOffReasonID As Integer
            Dim nCurrencyGainLossReasonID As Integer
            lComReturnValue = obACTAllocate.PerformAutoAllocation(OAllocation.AccountKey,
                                                                      OAllocation.LeadAllocatingTrans.TransDetailKey,
                                                                      r_oOtherAllocationTrans,
                                                                      OAllocation.LeadAllocatingTrans.CashListItemKey,
                                                                      nWriteOffReasonID,
                                                                      crWriteOffAmount,
                                                                      nCurrencyGainLossReasonID,
                                                                      crCurrencyGainLossAutoAllocaitonAmount)

            'This is not being used after this method call is completed
            'Fully allocated amount can be set here
            OAllocation.LeadAllocatingTrans.Amount = Math.Abs(OAllocation.LeadAllocatingTrans.Amount) +
            crWriteOffAmount +
            crCurrencyGainLossAutoAllocaitonAmount
        Else
            lComReturnValue = obACTAllocate.PerformAutoAllocation(OAllocation.AccountKey,
        OAllocation.LeadAllocatingTrans.TransDetailKey, r_oOtherAllocationTrans,
        OAllocation.LeadAllocatingTrans.CashListItemKey)
        End If

        If (lComReturnValue <> PMReturnCode.PMTrue) Then
            RaiseComMethodException("bACTAllocate.Business.PerformAutoAllocation",
            lComReturnValue)
        End If
        obACTAllocate.Dispose()
        obACTAllocate = Nothing

    End Sub

    ''' <summary>
    ''' Get the other allocation Trans from the database
    '''</summary>
    '''<param name="conPayClaim" >This is an object of a class SiriusConnection </param>   
    '''<param name="OAllocation">This is an object of a class BaseAllocationType</param>  
    '''<remarks></remarks>
    Private Sub GetTransForAllocationForAccount(ByVal conPayClaim As SiriusConnection,
   ByRef OAllocation As BaseAllocationType)
        Dim dsPayClaim As New DataSet
        Dim iCount As Integer = 0

        'Still there are some clarification needed in this section
        If (OAllocation IsNot Nothing) Then
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_ACT_Select_trans_for_allocation")
                cmd.AddInParameter("@account_id", SqlDbType.Int).Value = Cast.NullIfDefault _
                (OAllocation.AccountKey)
                cmd.AddInParameter("@company_id", SqlDbType.Int).Value = Cast.NullIfDefault _
                (OAllocation.SourceID)
                dsPayClaim = conPayClaim.ExecuteDataSet(cmd, "PayClaim")
            End Using
            If (dsPayClaim IsNot Nothing) Then
                If (dsPayClaim.Tables.Count > 0) Then
                    If (dsPayClaim.Tables(0).Rows.Count > 0) Then
                        ReDim OAllocation.OtherAllocatingTrans(dsPayClaim.Tables(0).Rows.Count - 1)
                        For iCount = 0 To dsPayClaim.Tables(0).Rows.Count - 1
                            OAllocation.OtherAllocatingTrans(iCount) = New BaseTransDetailType
                            OAllocation.OtherAllocatingTrans(iCount).TransDetailKey = Cast.ToInt32 _
                            (dsPayClaim.Tables(0).Rows(iCount).Item("transdetail_id"), 0)
                            OAllocation.OtherAllocatingTrans(iCount).Amount = Cast.ToDecimal _
                            (dsPayClaim.Tables(0).Rows(iCount).Item("outstanding_amount"), 0)
                        Next
                    End If
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' In this function maily the leadAllocationTrans is getting updated 
    '''</summary>
    '''<param name="oBusiness" >This is an object of a class CoreBusiness</param>   
    '''<param name="conPayClaim" >This is an object of a class SiriusConnection </param>   
    '''<param name="oCashList" ></param> 
    '''<param name="oCashListItem" >This is an object of a class BaseCoreCashListItemType</param> 
    '''<remarks></remarks>
    Private Sub AllocateCashListItem(ByVal oBusiness As CoreBusiness, ByVal conPayClaim As SiriusConnection,
    ByVal oCashList As BaseCoreCashListType, ByVal oCashListItem As BaseCoreCashListItemType)
        Dim oSAMErrorCollection As New SAMErrorCollection
        Dim oCoreBusiness As New CoreBusiness
        Dim crAllocatedAmount As Decimal
        'Rk modifies as part of SAM SFI Interop conversions.

        'If the cashList or cashListItem object is empty then error and exit.
        If (oCashList Is Nothing Or oCashListItem Is Nothing) Then
            oSAMErrorCollection.AddBusinessRule(SAMConstants.SAMInvalidData.MandatoryInputMissing,
             "CashList or CashListItem is empty", "CashList Or CashListItem")
        End If
        oSAMErrorCollection.CheckForErrors()

        'Recast the cashList param to it’s true type (BasePaymentCashListType or BaseReceiptCashListType).
        'If another type is detected then error and exit.
        If oCashList.GetType Is GetType(BasePaymentCashListType) Then

            If oCashListItem.GetType Is GetType(BasePaymentCashListItemType) Then
                'Do nothing
            ElseIf oCashListItem.GetType Is GetType(BaseReceiptCashListItemType) Then
                'Currently, we are not processing receipts in this method
                'Add invalid data error  - payment cash list can not contain reciept cash list item 
                oSAMErrorCollection.AddInvalidData(SAMInvalidData.ReceiptCashListItemProcessingIsNotSupported,
                                                  "Cash list of type Payment can not contain cash list item of type Receipt",
                                                   "CashListItem")
            Else
                'Unknown CashList item type
                'Add invalid data error  -invalid cashlist item type specified
                oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidCashListItemType,
                                                   "Unknown cashlist item type",
                                                   "CashListItem")
            End If

        ElseIf oCashList.GetType Is GetType(BaseReceiptCashListType) Then
            Dim oReceiptCashList As BaseReceiptCashListType
            oReceiptCashList = DirectCast(oCashList, BaseReceiptCashListType)

            If oCashListItem.GetType Is GetType(BaseReceiptCashListItemType) Then

                Dim oReceiptCashListItem As BaseReceiptCashListItemType
                oReceiptCashListItem = DirectCast(oCashListItem, BaseReceiptCashListItemType)

            Else
                'Unknown CashList item type
                'Add invalid data error  -invalid cashlist item type specified
                oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidCashListItemType,
                                                   "Unknown cashlist item type",
                                                   "CashListItem")
            End If
        Else
            'Unknown CashList type
            'Add invalid data error  -invalid cashlist type specified
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidCashListType,
                                               "Unknown cashlist type",
                                               "CashList")
        End If
        oSAMErrorCollection.CheckForErrors()

        'Load additional details from the CashList into the cashListItem.AllocationDetails object :
        oCashListItem.AllocationDetails.BranchCode = oCashList.BranchCode
        oCashListItem.AllocationDetails.SourceID = oCashList.SourceID

        'Load additional details from the CashListItem into the cashListItem.AllocationDetails object :
        oCashListItem.AllocationDetails.AccountKey = oCashListItem.AccountKey
        oCashListItem.AllocationDetails.LeadAllocatingTrans = New BaseTransDetailType
        oCashListItem.AllocationDetails.LeadAllocatingTrans.TransDetailKey = oCashListItem.TransDetailKey
        oCashListItem.AllocationDetails.LeadAllocatingTrans.CashListItemKey = oCashListItem.CashListItemKey
        oCashListItem.AllocationDetails.LeadAllocatingTrans.Amount = oCashListItem.Amount
        If oCashList.GetType Is GetType(BaseReceiptCashListType) AndAlso oCashListItem.AllocationDetails IsNot Nothing AndAlso Not oCashListItem.AllocationDetails.AutoAllocate Then
            Dim oUpdateAllocationRequest As SAMForInsuranceV2ImplementationTypes.UpdateAllocationRequestType
            oUpdateAllocationRequest = New SAMForInsuranceV2ImplementationTypes.UpdateAllocationRequestType
            oUpdateAllocationRequest.AccountKey = oCashListItem.AccountKey
            oUpdateAllocationRequest.BranchCode = oCashList.BranchCode
            oUpdateAllocationRequest.TransdetailKey = oCashListItem.TransDetailKey
            oUpdateAllocationRequest.Amount = oCashListItem.Amount
            oUpdateAllocationRequest.CashListItemKey = oCashListItem.CashListItemKey
            Dim iCount As Integer = 0
            With oCashListItem.AllocationDetails
                If Not (.OtherAllocatingTrans Is Nothing) Then
                    Dim oAllocation As BaseTransDetailType
                    ReDim oUpdateAllocationRequest.Allocation(.OtherAllocatingTrans.Length - 1)
                    For Each oAllocation In .OtherAllocatingTrans
                        oUpdateAllocationRequest.Allocation(iCount) = New BaseImplementationTypes.BaseUpdateAllocationRequestTypeAllocation
                        oUpdateAllocationRequest.Allocation(iCount).AllocationTransdetailKey = oAllocation.TransDetailKey
                        oUpdateAllocationRequest.Allocation(iCount).AllocationAmount = oAllocation.Amount

                        Dim AnyError As STSErrorType
                        Dim bIsLocked As Boolean
                        Dim btimestamp As Byte = Nothing
                        AnyError = oCoreBusiness.GetTimestamp(conPayClaim,
                                            oCashList.BranchCode,
                                            CoreBusiness.LockName.TransDetailKey,
                                            oAllocation.TransDetailKey,
                                             oUpdateAllocationRequest.Allocation(iCount).AllocationTimeStamp,
                                            bIsLocked)

                        iCount = iCount + 1
                        oCoreBusiness = Nothing
                    Next
                End If
            End With

            UpdateAllocation(conPayClaim, oUpdateAllocationRequest)
            crAllocatedAmount = oCashListItem.Amount * -1
        Else

            'Allocate the CashListItem by passing the cashListItem.AllocationDetails object to 
            'the ProcessAllocation method and deal with any errors by calling sAMErrorCollection.CheckForErrors
            ProcessAllocation(oBusiness, conPayClaim, oCashListItem.AllocationDetails, oSAMErrorCollection, crAllocatedAmount)
            oSAMErrorCollection.CheckForErrors()
        End If
        Dim dAmountAllocated As Decimal
        Dim lAllocationStatusKey As Integer
        'Check whether the allocation worked by passing the following parameters to 
        Dim obACTCashListItem As New bACTCashListItem.Form
        Dim lReturn As Integer = 0
        Dim lComReturnValue As Integer = 0
        'This portion will call the com method to initialize
        'Rk modifies as part of SAM SFI Interop conversions by replacing .PMDAODatabase by .FromSirius.SqlConnection.Database for vDatabase parameter.
        Dim oDatabase As Object = Nothing
        If Not conPayClaim Is Nothing Then
            oDatabase = Nothing
            oDatabase = conPayClaim.PMDAODatabase
        End If
        lReturn = obACTCashListItem.Initialise(
                                                _SiriusUser.Username,
                                                _SiriusUser.Password,
                                                CShort(_SiriusUser.UserID),
                                                CShort(_SiriusUser.SourceID),
                                                CShort(_SiriusUser.LanguageID),
                                                CShort(_SiriusUser.CurrencyID),
                                                1,
                                                SiriusUserDefaults.AppName,
                                                vDatabase:=oDatabase)

        If (lReturn <> PMEReturnCode.PMTrue) Then

            ' if the account processing fails then throw a business rule error
            oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed,
                                                SAMBusinessErrors.COMComponentMethodFailed.ToString,
                                                "obACTCashListItem.Initialise")
            oSAMErrorCollection.CheckForErrors()

        End If
        If oCashList.GetType Is GetType(BaseReceiptCashListType) Then
            'Note:(oCashListItem.Amount * -1), this conversion is done based on the backoffice and it is confirmed with Qaurav
            lComReturnValue = obACTCashListItem.GetMultiAllocationStatus(oCashListItem.CashListItemKey,
            Cast.ToInt16(oCashListItem.AllocationDetails.OtherAllocatingTrans(0).AmountCurrencyId, 0),
            (crAllocatedAmount), lAllocationStatusKey, dAmountAllocated)
        Else
            'Note:(oCashListItem.Amount * -1), this conversion is done based on the backoffice and it is confirmed with Qaurav
            lComReturnValue = obACTCashListItem.GetMultiAllocationStatus(oCashListItem.CashListItemKey,
            Cast.ToInt16(oCashList.CurrencyKey, 0),
            (crAllocatedAmount), lAllocationStatusKey, dAmountAllocated)
        End If
        If (lComReturnValue <> PMEReturnCode.PMTrue) Then

            ' if the account processing fails then throw a business rule error
            oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed,
                                                SAMBusinessErrors.COMComponentMethodFailed.ToString,
                                                "obACTCashListItem.GetMultiAllocationStatus")
            oSAMErrorCollection.CheckForErrors()

        End If
        'If the allocation status has changed (ie the returned r_lAllocationStatus param <>  _
        'cashListItem.AllocationStatusKey) then

        If (lAllocationStatusKey <> 0 AndAlso lAllocationStatusKey <> oCashListItem.AllocationStatusKey) Then
            'Update the cashListItem object as follows :
            oCashListItem.AllocationStatusKey = lAllocationStatusKey

            'Set the cashListItem.AllocationStatusCode to match the key above by calling 
            'business.GetListItemFromID against PMLookup table AllocationStatus
            oCashListItem.AllocationStatusCode = oBusiness.GetListItemFromID(Core.STSListType.PMLookup, PMLookupTable.AllocationStatus, oCashListItem.AllocationStatusKey)
            'Save the updated allocation status to the database by passing the following 
            'parameters to stored procedure spu_SAM_Update_CashListItem_Allocation_Status
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Update_CashListItem_Allocation_Status")
                cmd.AddInParameter("@CashListItem_ID", SqlDbType.Int).Value = Cast.NullIfDefault(oCashListItem.CashListItemKey)
                cmd.AddInParameter("@AllocationStatus_ID", SqlDbType.Int).Value = Cast.NullIfDefault(oCashListItem.AllocationStatusKey)
                conPayClaim.ExecuteNonQuery(cmd)
            End Using
        End If
        obACTCashListItem.Dispose()
        obACTCashListItem = Nothing
    End Sub

    ''' <summary>
    ''' This function will validate the Payment Item details
    '''</summary>
    '''<param name="oBusiness" >This is an object of a class CoreBusiness </param>   
    '''<param name="conPayClaim"> This is an object of aSiriusConnection </param>   
    '''<param name="oPaymentCashList" >This is an object of a BasePaymentCashListType</param> 
    '''<param name="oPaymentItem" >This is an object of a BasePaymentCashListItemType</param> 
    '''<param name="oSAMErrorCollection">This is an object of a SAMErrorCollection</param>
    '''<remarks></remarks>
    Private Sub ValidateBasePaymentCashListItemType(ByVal oBusiness As CoreBusiness, ByVal _
    conPayClaim As SiriusConnection, ByRef oPaymentCashList As BasePaymentCashListType,
    ByRef oPaymentItem As BasePaymentCashListItemType, ByRef oSAMErrorCollection As SAMErrorCollection, ByVal sResultMultiStepApproval As String)

        ValidateBaseCoreCashListItemType(conPayClaim, oBusiness, DirectCast(oPaymentCashList, BaseCoreCashListType),
        DirectCast(oPaymentItem, BaseCoreCashListItemType), oSAMErrorCollection, sResultMultiStepApproval)
        oSAMErrorCollection.CheckForErrors()

        oPaymentItem.Validate(CObj(oSAMErrorCollection))
        oSAMErrorCollection.CheckForErrors()

        Dim iMediatypeValidationId As Integer = 0
        Dim sMediatypeValidation As String

        If oPaymentItem IsNot Nothing Then
            With oPaymentItem
                If String.IsNullOrEmpty(.TypeCode) = False Then
                    .TypeKey = oBusiness.GetAndValidateListItemFromCode(
                                                                        Core.STSListType.PMLookup,
                                                                        PMLookupTable.CashListItemPaymentType,
                                                                        .TypeCode,
                                                                        "TypeCode",
                                                                        oSAMErrorCollection)
                End If
                If String.IsNullOrEmpty(.StatusCode) = False Then
                    .StatusKey = oBusiness.GetAndValidateListItemFromCode(
                                                                        Core.STSListType.PMLookup,
                                                                        PMLookupTable.CashListItemPaymentStatus,
                                                                        .StatusCode,
                                                                        "StatusCode",
                                                                        oSAMErrorCollection)
                End If

                If String.IsNullOrEmpty(.MediaTypeCode) = False Then
                    .MediaTypeKey = GetAndValidateSpecifiedTableCode(conPayClaim,
                                                                    PMLookupTable.MediaTypePayment,
                                                                    "MediaType_Id",
                                                                    "code",
                                                                    .MediaTypeCode,
                                                                    oSAMErrorCollection, "MediaTypeCode")
                End If

                'Validate data specific to Payments
                iMediatypeValidationId = GetAndValidateSpecifiedTableCode(conPayClaim,
                "MediaType", "MediaType_Validation_Id", "Code",
                .MediaTypeCode, oSAMErrorCollection, "MediaType_Validation")

                oSAMErrorCollection.CheckForErrors()

                sMediatypeValidation = oBusiness.GetListItemFromID(Core.STSListType.PMLookup,
                PMLookupTable.MediaTypeValidationType, iMediatypeValidationId)

                If (sMediatypeValidation <> MediaTypeValidationType.CreditCard) Then
                    If (.CreditCard IsNot Nothing) Then
                        oSAMErrorCollection.AddInvalidData(
                        SAMConstants.SAMInvalidData.InvalidFormat,
                        "Credit Card details have been input for an inappropriate media type",
                        "MediaTypeValidation")
                        oSAMErrorCollection.CheckForErrors()
                    End If

                    'If bank details provided along with party bank key, check for validity
                    If .Bank IsNot Nothing AndAlso .Bank.PartyBankKey > 0 Then

                        'Validate bank item
                        ValidatePartyBank(conPayClaim, oSAMErrorCollection, .Bank.PartyBankKey, Nothing, .AccountKey, True, .Bank.AccountCode)

                        oSAMErrorCollection.CheckForErrors()
                    End If

                End If

                If (sMediatypeValidation = MediaTypeValidationType.CreditCard Or sMediatypeValidation _
                = MediaTypeValidationType.Cash) Then
                    'If (.Bank IsNot Nothing) Then
                    '    oSAMErrorCollection.AddInvalidData( _
                    '    SAMConstants.SAMInvalidData.InvalidFormat, _
                    '    "Bank details have been input for an inappropriate media type", _
                    '    "MediaTypeValidation")
                    '    oSAMErrorCollection.CheckForErrors()
                    'End If

                    'If party bank key is provided for credit card item, check its validity
                    If .CreditCard IsNot Nothing AndAlso .CreditCard.PartyBankKey > 0 Then
                        'Validate the party bank
                        ValidatePartyBank(conPayClaim, oSAMErrorCollection, .CreditCard.PartyBankKey, Nothing, .AccountKey, False, .CreditCard.Number)

                        oSAMErrorCollection.CheckForErrors()
                    End If

                End If

                '****************************************************************************************                
                'Note that AccountCode / BranchCode can be validated using a configurable script 
                'via bSIRMediaTypeValidation.Business.ValidateNumber but is out of scope at the moment. 
                '****************************************************************************************

                'If the payment is via credit card (CreditCard object contains data) then Validate the 
                'CreditCard data by passing the BaseCreditCardType object to ValidateBaseCreditCardTypeData

                If .CreditCard IsNot Nothing AndAlso (.MediaTypeCode = MediaTypeValidationType.CreditCard) Then
                    ValidateBaseCreditCardTypeData(.CreditCard, oSAMErrorCollection)

                    'Prakash: after validation, check for errors
                    oSAMErrorCollection.CheckForErrors()
                End If
            End With
        End If
    End Sub

    ''' <summary>
    ''' This function will validate the Payment Item details
    '''</summary>
    '''<param name="oBusiness" >This is an object of a class CoreBusiness </param>   
    '''<param name="conReceiptClaim"> This is an object of a SiriusConnection </param>   
    '''<param name="oReceiptCashList" >This is an object of a BaseReceiptCashListType</param> 
    '''<param name="oReceiptItem" >This is an object of a BaseReceiptCashListItemType</param> 
    '''<param name="oSAMErrorCollection">This is an object of a SAMErrorCollection</param>
    '''<remarks></remarks>
    Private Sub ValidateBaseReceiptCashListItemType(ByVal oBusiness As CoreBusiness, ByVal _
    conReceiptClaim As SiriusConnection, ByRef oReceiptCashList As BaseReceiptCashListType,
    ByRef oReceiptItem As BaseReceiptCashListItemType, ByRef oSAMErrorCollection As SAMErrorCollection)

        ValidateBaseCoreCashListItemType(conReceiptClaim, oBusiness, DirectCast(oReceiptCashList, BaseCoreCashListType),
        DirectCast(oReceiptItem, BaseCoreCashListItemType), oSAMErrorCollection)
        oSAMErrorCollection.CheckForErrors()

        oReceiptItem.Validate(CObj(oSAMErrorCollection))
        oSAMErrorCollection.CheckForErrors()

        Dim iMediatypeValidationId As Integer = 0
        Dim sMediatypeValidation As String

        If oReceiptItem IsNot Nothing Then
            With oReceiptItem
                If String.IsNullOrEmpty(.TypeCode) = False Then
                    .TypeKey = oBusiness.GetAndValidateListItemFromCode(
                                                                        Core.STSListType.PMLookup,
                                                                        PMLookupTable.CashListItemReceiptType,
                                                                        .TypeCode,
                                                                        "TypeCode",
                                                                        oSAMErrorCollection)
                End If
                If String.IsNullOrEmpty(.StatusCode) = False Then
                    .StatusKey = oBusiness.GetAndValidateListItemFromCode(
                                                                        Core.STSListType.PMLookup,
                                                                        PMLookupTable.CashListItemReceiptStatus,
                                                                        .StatusCode,
                                                              "StatusCode",
                                                                        oSAMErrorCollection)
                End If

                If String.IsNullOrEmpty(.MediaTypeCode) = False Then
                    .MediaTypeKey = GetAndValidateSpecifiedTableCode(conReceiptClaim,
                                                                    PMLookupTable.MediaTypeReceipt,
                                                                    "MediaType_Id",
                                                                    "code",
                                                                    .MediaTypeCode,
                                                                    oSAMErrorCollection, "MediaTypeCode")
                End If

                'Validate data specific to Payments
                iMediatypeValidationId = GetAndValidateSpecifiedTableCode(conReceiptClaim,
                "MediaType", "MediaType_Validation_Id", "Code",
                .MediaTypeCode, oSAMErrorCollection, "MediaType_Validation")
                sMediatypeValidation = oBusiness.GetListItemFromID(Core.STSListType.PMLookup,
                PMLookupTable.MediaTypeValidationType, iMediatypeValidationId)

                If (sMediatypeValidation <> MediaTypeValidationType.CreditCard) Then
                    If (.CreditCard IsNot Nothing) Then
                        oSAMErrorCollection.AddInvalidData(
                        SAMConstants.SAMInvalidData.InvalidDateFormat,
                        "Credit Card details have been input for an inappropriate media type",
                        "MediaTypeValidation")

                        oSAMErrorCollection.CheckForErrors()
                    End If

                    'If party bank key is provided for bank item, check its validity. We can not validate bank account number since it is not present in the structure
                    If .Bank IsNot Nothing AndAlso .Bank.PartyBankKey > 0 Then

                        'Validate bank item
                        ValidatePartyBank(conReceiptClaim, oSAMErrorCollection, .Bank.PartyBankKey, Nothing, .AccountKey)

                        oSAMErrorCollection.CheckForErrors()
                    End If

                End If

                If (sMediatypeValidation = MediaTypeValidationType.CreditCard Or sMediatypeValidation _
                = MediaTypeValidationType.Cash) Then
                    If (.Bank IsNot Nothing) Then
                        oSAMErrorCollection.AddInvalidData(
                        SAMConstants.SAMInvalidData.InvalidDateFormat,
                        "Bank details have been input for an inappropriate media type",
                        "MediaTypeValidation")

                        oSAMErrorCollection.CheckForErrors()
                    End If

                    'If party bank key is provided for credit card item, check its validity
                    If .CreditCard IsNot Nothing AndAlso .CreditCard.PartyBankKey > 0 Then

                        'Validate credit card item
                        ValidatePartyBank(conReceiptClaim, oSAMErrorCollection, .CreditCard.PartyBankKey, Nothing, .AccountKey, False, .CreditCard.Number)

                        oSAMErrorCollection.CheckForErrors()
                    End If

                End If

                oSAMErrorCollection.CheckForErrors()

                If oReceiptItem.InstalmentPlanDetails IsNot Nothing Then
                    Dim iAccountId As Integer
                    Dim dsResult As DataSet = Nothing
                    Dim dAmount As Decimal
                    Dim oErrors As New SAMErrorCollection
                    Using conFP As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
                        If Not String.IsNullOrEmpty(oReceiptItem.AccountShortCode) Then
                            iAccountId = GetAndValidateSpecifiedTableCode(conFP, "account",
                                    "account_id", "Short_Code", oReceiptItem.AccountShortCode.ToString(), oErrors, "short_code")
                        End If

                        Using cmdInstallement As SiriusCommand = SiriusCommand.FromProcedure("spu_ACT_Select_Instalments_For_Account")

                            cmdInstallement.AddInParameter("@account_id", SqlDbType.Int).Value = iAccountId
                            dsResult = conFP.ExecuteDataSet(cmdInstallement, "SelectInstallmentDetails")
                        End Using
                    End Using

                    If dsResult.Tables.Count > 0 Then
                        If dsResult.Tables(0).Rows.Count > 0 Then
                            For iCount As Integer = 0 To oReceiptItem.InstalmentPlanDetails.GetUpperBound(0)
                                For iCount1 As Integer = 0 To dsResult.Tables(0).Rows.Count - 1
                                    If oReceiptItem.InstalmentPlanDetails(iCount).FinancePlanKey = Cast.ToInt32(dsResult.Tables(0).Rows(iCount1).Item("pfprem_finance_cnt"), 0) And oReceiptItem.InstalmentPlanDetails(iCount).FinancePlanVersion = Cast.ToInt32(dsResult.Tables(0).Rows(iCount1).Item("pfprem_finance_version"), 0) Then
                                        If oReceiptItem.InstalmentPlanDetails(iCount).InstalmentDetails IsNot Nothing Then
                                            For iCount2 As Integer = 0 To oReceiptItem.InstalmentPlanDetails(iCount).InstalmentDetails.GetUpperBound(0)
                                                If oReceiptItem.InstalmentPlanDetails(iCount).InstalmentDetails(iCount2).InstalmentNumber = Cast.ToInt32(dsResult.Tables(0).Rows(iCount1).Item("instalmentnumber"), 0) Then
                                                    If Math.Round(oReceiptItem.InstalmentPlanDetails(iCount).InstalmentDetails(iCount2).Amount, 2) <= Math.Round(Cast.ToDouble(dsResult.Tables(0).Rows(iCount1).Item("amount"), 0.0), 2) Then
                                                        oReceiptItem.InstalmentPlanDetails(iCount).InstalmentDetails(iCount2).iPFInstalmentID = Cast.ToInt32(dsResult.Tables(0).Rows(iCount1).Item("pfinstalments_id"), 0)
                                                    Else
                                                        oSAMErrorCollection.AddInvalidData(
                                                                                           SAMConstants.SAMInvalidData.InvalidDateFormat,
                                                                                           "The amounts of each instalment specified  should be less than or equal to the Amount specified in the installment plans",
                                                                                           "Amount")
                                                    End If
                                                    oSAMErrorCollection.CheckForErrors()
                                                End If
                                            Next
                                        End If
                                    End If
                                Next
                            Next
                        End If
                    End If
                    Dim bIsOverPaymentWriteOff As Boolean = False
                    If oReceiptItem.InstalmentPlanDetails IsNot Nothing Then
                        For iCount3 As Integer = 0 To oReceiptItem.InstalmentPlanDetails.GetUpperBound(0)
                            If oReceiptItem.InstalmentPlanDetails(iCount3).InstalmentDetails IsNot Nothing Then
                                For iCount4 As Integer = 0 To oReceiptItem.InstalmentPlanDetails(iCount3).InstalmentDetails.GetUpperBound(0)
                                    dAmount = dAmount + oReceiptItem.InstalmentPlanDetails(iCount3).InstalmentDetails(iCount4).Amount
                                    If (oReceiptItem.InstalmentPlanDetails(iCount3).InstalmentDetails(iCount4).OverPaymentWriteOffAmount <> 0.0) Then
                                        bIsOverPaymentWriteOff = True
                                        Exit For
                                    End If
                                Next
                            End If
                        Next
                    End If
                    If oReceiptItem.TypeCode = "INST" Then
                        If oReceiptItem.Amount > Math.Round(dAmount, 2) AndAlso (Not bIsOverPaymentWriteOff) Then
                            oSAMErrorCollection.AddInvalidData(
                                                               SAMConstants.SAMInvalidData.InvalidDateFormat,
                                                               "The total amount of the cashlistitem instalment amounts should be  less Than or equal to the amount on the cashlistitem=",
                                                               "Amount")
                        End If
                    End If
                    oSAMErrorCollection.CheckForErrors()

                End If

                '****************************************************************************************                
                'Note that AccountCode / BranchCode can be validated using a configurable script 
                'via bSIRMediaTypeValidation.Business.ValidateNumber but is out of scope at the moment. 
                '****************************************************************************************

                'If the payment is via credit card (CreditCard object contains data) then Validate the 
                'CreditCard data by passing the BaseCreditCardType object to ValidateBaseCreditCardTypeData

                If (.MediaTypeCode = MediaTypeValidationType.CreditCard) Then
                    ValidateBaseCreditCardTypeData(.CreditCard, oSAMErrorCollection)
                End If

            End With
        End If
    End Sub

    ''' <summary>
    ''' This function will validate the Payment Item details
    '''</summary>
    '''<param name="oPayment"  >This is an object to a class BaseClaimPaymentType</param>
    '''<param name="oSAMErrorCollection" >This is an object to a class SAMErrorCollection</param>
    '''<remarks></remarks>
    Public Sub ClaimPaymentWorkFlowValidation(ByVal oPayment As BaseClaimPaymentType, ByRef oSAMErrorCollection As SAMErrorCollection, ByRef oClaimResponsewarning As BaseClaimResponseType)

        If (oPayment.ClaimPaymentWorkflowEnabled = False AndAlso oPayment.CashList IsNot Nothing) Then
            'oSAMErrorCollection.AddBusinessRule(SAMConstants.SAMBusinessWarnings.WorkFlowIsDisabledButCashListIsNotEmpty, _
            '   "Cash list data cannot be submitted, as the claim payment work flow has not been enabled in Back office - CashList data will be ignored", _
            '   "CashList")
            AddWarning(oClaimResponsewarning, SAMConstants.SAMBusinessWarnings.WorkFlowIsDisabledButCashListIsNotEmpty, "Cash list data cannot be submitted, as the claim payment work flow has not been enabled in Back office - CashList data will be ignored")
        End If

        If (oPayment.ClaimPaymentWorkflowEnabled = True And oPayment.CashList Is Nothing) Then
            oSAMErrorCollection.AddBusinessRule(SAMConstants.SAMBusinessErrors.CashListIsEmptyButWorkFlowEnabled,
               "Workflow is enabled for claim payments but CashList data is missing",
               "CashList")
        End If

        'Prakash: Added check on Cashlist before accessing its memebers.
        If (oPayment.CashList IsNot Nothing) Then
            If (oPayment.CashList.PaymentItem.Length = 0) Then
                oSAMErrorCollection.AddBusinessRule(SAMConstants.SAMBusinessErrors.ClaimPaymentIsNotAllowedAgainstAnInfoOnlyClaim,
                   "Payment item is missing from CashList",
                   "PaymentItem")

            End If

            If (oPayment.CashList.TypeCode <> CashListType.ClaimPayment) Then
                oSAMErrorCollection.AddBusinessRule(SAMConstants.SAMBusinessErrors.ClaimPaymentIsNotAllowedAgainstAnInfoOnlyClaim,
                   "CashList Type Code is invalid for a claim payment",
                   "Claim Payment")

            End If
        End If
    End Sub

    ''' <summary>
    ''' Processes the cashlist
    ''' </summary>
    '''<param name="con">An object of SiriusConnection class</param>
    '''<param name="oBusiness">An object of CoreBusiness class</param>
    '''<param name="oCashList">An object of BaseCoreCashListType class</param>
    '''<param name="oSAMErrorCollection">An object of SAMErrorCollection class</param>
    '''<param name="bIsValidated">An boolean value which specifies the cashlist validation is already done or not</param>
    Private Sub ProcessCashList(ByVal con As SiriusConnection,
                                ByVal oBusiness As CoreBusiness,
                                ByRef oCashList As BaseCoreCashListType,
                                ByRef oSAMErrorCollection As SAMErrorCollection,
                                Optional ByVal bIsValidated As Boolean = False,
                                Optional ByVal bIsClaimPayment As Boolean = False,
                                Optional ByVal lClaimPaymentKey As Integer = 0,
                                Optional ByVal bNoAuthority As Boolean = False,
                                Optional ByVal bSettlePayment As Boolean = False)
        'Deviation from TechSpec: Added one optional parameter to indicate Cashlist object validation is required or not
        If oCashList.GetType Is GetType(BasePaymentCashListType) Then
            Dim oPaymentCashList As BasePaymentCashListType
            oPaymentCashList = DirectCast(oCashList, BasePaymentCashListType)

            If Not bIsValidated Then
                ValidateCashList(con, oBusiness, oCashList, oSAMErrorCollection)
                'Check for Errors
                oSAMErrorCollection.CheckForErrors()
            End If

            If (oPaymentCashList.CashListKey <> 0) Then
                'Currently, this application supports creation of new cash list, not modification of existing cash list.
                'If oCashList.CashListKey is not zero then the cashlist is not a new cash list.
                'Add invalid data error  - cashlist updation is not supported
                oSAMErrorCollection.AddInvalidData(SAMInvalidData.CashListUpdationIsNotSupported,
                                                   "Cashlist updation is not supported",
                                                   "CashList")
            Else

                If Not bIsClaimPayment Then
                    con.BeginTransaction()
                End If
                Try

                    Dim dAmount As Double = 0
                    If bIsClaimPayment Then
                        For Each oCashListItem As BasePaymentCashListItemType In oPaymentCashList.PaymentItem
                            dAmount = dAmount + oCashListItem.Amount
                        Next
                        If dAmount < 0 Then
                            oCashList.TypeCode = "R"
                            oCashList.TypeKey = 2
                        End If
                    End If
                    CreateCashList(con, oCashList, oSAMErrorCollection)
                    oSAMErrorCollection.CheckForErrors()

                    If oPaymentCashList.PaymentItem IsNot Nothing Then
                        For Each oCashListItem As BasePaymentCashListItemType In oPaymentCashList.PaymentItem
                            ProcessCashListItem(con, oBusiness, oCashList, DirectCast(oCashListItem, BaseCoreCashListItemType), oSAMErrorCollection, bIsClaimPayment, lClaimPaymentKey, bNoAuthority, bSettlePayment)
                            oSAMErrorCollection.CheckForErrors()
                        Next
                    End If
                    If Not bIsClaimPayment Then
                        con.CommitTransaction()
                    End If
                Catch ex As Exception
                    If Not bIsClaimPayment Then
                        con.RollbackTransaction()
                    End If
                    Throw
                End Try

            End If
        ElseIf oCashList.GetType Is GetType(BaseReceiptCashListType) Then

            Dim oReceiptCashList As BaseReceiptCashListType
            oReceiptCashList = DirectCast(oCashList, BaseReceiptCashListType)

            If (oReceiptCashList.CashListKey <> 0) Then
                'Currently, this application supports creation of new cash list, not modification of existing cash list.
                'If oCashList.CashListKey is not zero then the cashlist is not a new cash list.
                'Add invalid data error  - cashlist updation is not supported
                oSAMErrorCollection.AddInvalidData(SAMInvalidData.CashListUpdationIsNotSupported,
                                                   "Cashlist updation is not supported",
                                                   "CashList")
            Else

                If Not bIsClaimPayment Then
                    con.BeginTransaction()
                End If
                Try
                    CreateCashList(con, oCashList, oSAMErrorCollection)
                    oSAMErrorCollection.CheckForErrors()

                    If oReceiptCashList.ReceiptItem IsNot Nothing Then
                        For Each oCashListItem As BaseReceiptCashListItemType In oReceiptCashList.ReceiptItem
                            ProcessCashListItem(con, oBusiness, oCashList, DirectCast(oCashListItem, BaseCoreCashListItemType), oSAMErrorCollection, bIsClaimPayment, lClaimPaymentKey, bNoAuthority, bSettlePayment)
                            oSAMErrorCollection.CheckForErrors()
                        Next
                    End If
                    If Not bIsClaimPayment Then
                        con.CommitTransaction()
                    End If
                Catch ex As Exception
                    If Not bIsClaimPayment Then
                        con.RollbackTransaction()
                    End If
                    Throw
                End Try

            End If
        Else
            'Unknown CashList type
            'Add invalid data error  -invalid cashlist type specified
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidCashListType,
                                               "Unknown cashlist type",
                                               "CashList")
        End If

    End Sub


    ''' <summary>
    ''' Gets outstanding transactions for given account and document
    ''' </summary>
    '''<param name="con">An object of SiriusConnection class</param>
    '''<param name="oBusiness">An object of CoreBusiness class</param>
    '''<param name="oPaymentItem">An object of BasePaymentCashListItemType class</param>
    '''<param name="iClaimPaymentAccountKey">Payment account key</param>
    '''<param name="iClaimPaymentDocumentKey">Payment document key</param>
    Private Sub GetTransForAllocationForDocument(ByVal con As SiriusConnection,
                                                 ByRef oPaymentItem As BasePaymentCashListItemType,
                                                 ByVal iClaimPaymentAccountKey As Integer,
                                                 ByVal iClaimPaymentDocumentKey As Integer)

        Dim dtTrans As New DataTable

        ' Get transaction details
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_ACT_Select_Trans_For_Allocation_For_Document")

            cmd.AddInParameter("@account_id", SqlDbType.Int).Value = iClaimPaymentAccountKey
            cmd.AddInParameter("@document_id", SqlDbType.VarChar, 40).Value = iClaimPaymentDocumentKey.ToString

            con.ExecuteDataTable(cmd, dtTrans)
        End Using

        ' If there are transaction detail records in the database 
        If dtTrans IsNot Nothing AndAlso dtTrans.Rows.Count > 0 Then
            ReDim oPaymentItem.AllocationDetails.OtherAllocatingTrans(dtTrans.Rows.Count - 1)
            For iCnt As Integer = 0 To dtTrans.Rows.Count - 1
                oPaymentItem.AllocationDetails.OtherAllocatingTrans(iCnt) = New BaseTransDetailType
                oPaymentItem.AllocationDetails.OtherAllocatingTrans(iCnt).TransDetailKey = Cast.ToInt32(dtTrans.Rows(iCnt).Item("transdetail_id"), 0)
                oPaymentItem.AllocationDetails.OtherAllocatingTrans(iCnt).Amount = Cast.ToDecimal(dtTrans.Rows(iCnt).Item("amount"), 0)
            Next
        End If
    End Sub
    ''' 
    ''' </summary>
    ''' <param name="con"></param>
    ''' <param name="oPaymentItem"></param>
    ''' <param name="iClaimPaymentAccountKey"></param>
    ''' <param name="iClaimPaymentDocumentKey"></param>
    ''' <remarks></remarks>
    Private Sub GetTransForAllocationForDocumentReceipt(ByVal con As SiriusConnection,
                                                 ByRef oReceiptItem As BaseReceiptCashListItemType,
                                                 ByVal iClaimReceiptAccountKey As Integer,
                                                 ByVal iClaimReceiptDocumentKey As Integer)

        Dim dtTrans As New DataTable

        ' Get transaction details
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_ACT_Select_Trans_For_Allocation_For_Document")

            cmd.AddInParameter("@account_id", SqlDbType.Int).Value = iClaimReceiptAccountKey
            cmd.AddInParameter("@document_id", SqlDbType.VarChar, 40).Value = iClaimReceiptDocumentKey.ToString

            con.ExecuteDataTable(cmd, dtTrans)
        End Using

        ' If there are transaction detail records in the database 
        If dtTrans IsNot Nothing AndAlso dtTrans.Rows.Count > 0 Then
            ReDim oReceiptItem.AllocationDetails.OtherAllocatingTrans(dtTrans.Rows.Count - 1)
            For iCnt As Integer = 0 To dtTrans.Rows.Count - 1
                oReceiptItem.AllocationDetails.OtherAllocatingTrans(iCnt) = New BaseTransDetailType
                oReceiptItem.AllocationDetails.OtherAllocatingTrans(iCnt).TransDetailKey = Cast.ToInt32(dtTrans.Rows(iCnt).Item("transdetail_id"), 0)
                oReceiptItem.AllocationDetails.OtherAllocatingTrans(iCnt).Amount = Cast.ToDecimal(dtTrans.Rows(iCnt).Item("amount"), 0)
                oReceiptItem.AllocationDetails.OtherAllocatingTrans(iCnt).AmountCurrencyId = Cast.ToInt32(dtTrans.Rows(iCnt).Item("amount_currency_id"), 0)
            Next
        End If
    End Sub

    ''' <summary>

    ''' <summary>
    ''' Validates the claim payment cashlist
    ''' </summary>
    '''<param name="con">An object of SiriusConnection class</param>
    '''<param name="oBusiness">An object of CoreBusiness class</param>
    '''<param name="oCashList">An object of BaseCoreCashListType class</param>
    '''<param name="oSAMErrorCollection">An object of oSAMErrorCollection class</param>
    '''<param name="iClaimPaymentDocumentKey">Payment document key</param>
    Private Sub ValidateCashList(ByVal con As SiriusConnection,
                                 ByVal oBusiness As CoreBusiness,
                                 ByRef oCashList As BaseCoreCashListType,
                                 ByRef oSAMErrorCollection As SAMErrorCollection)

        If oCashList.GetType Is GetType(BasePaymentCashListType) Then
            Dim oPaymentCashList As BasePaymentCashListType
            oPaymentCashList = DirectCast(oCashList, BasePaymentCashListType)

            ValidateBasePaymentCashListType(con, oBusiness, oPaymentCashList, oSAMErrorCollection)
            oSAMErrorCollection.CheckForErrors()

        ElseIf oCashList.GetType Is GetType(BaseReceiptCashListType) Then
            'Currently, we are not processing receipts in this method
            'Add invalid data error  - receipt cashList processing is not supported 
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.ReceiptCashListProcessingIsNotSupported,
                                               "Receipt handling is not supported",
                                               "CashList")

        Else
            'Unknown CashList type
            'Add invalid data error  -invalid cashlist type specified
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidCashListType,
                                               "Unknown cashlist type",
                                               "CashList")
        End If

    End Sub

    ''' <summary>
    '''  Validates the base payment cashlist
    ''' </summary>
    '''<param name="con">An object of SiriusConnection class</param>
    '''<param name="oBusiness">An object of CoreBusiness class</param>
    '''<param name="oCashList">An object of BasePaymentCashListType class</param>
    '''<param name="oSAMErrorCollection">An object of SAMErrorCollection class</param>
    Private Sub ValidateBasePaymentCashListType(ByVal con As SiriusConnection,
                                                ByVal oBusiness As CoreBusiness,
                                                ByRef oCashList As BasePaymentCashListType,
                                                ByRef oSAMErrorCollection As SAMErrorCollection)
        Dim sResultMultiStepApproval As String = "0"
        ValidateBaseCoreCashListType(con, oBusiness, DirectCast(oCashList, BaseCoreCashListType), oSAMErrorCollection)

        oCashList.Validate(CObj(oSAMErrorCollection))
        'Check for multiStepApproval Process
        sResultMultiStepApproval = oBusiness.GetProductOption(SIRHiddenOptions.SIROPTMultiStepApproval, 1)
        If (oCashList.PaymentItem IsNot Nothing) Then
            For Each oPaymentItem As BasePaymentCashListItemType In oCashList.PaymentItem
                ValidateBasePaymentCashListItemType(oBusiness, con, oCashList, oPaymentItem, oSAMErrorCollection, sResultMultiStepApproval)
            Next
        End If
    End Sub

    ''' <summary>
    ''' Validates core cashlist 
    ''' </summary>
    '''<param name="con">An object of SiriusConnection class</param>
    '''<param name="oBusiness">An object of CoreBusiness class</param>
    '''<param name="oCashList">An object of BaseCoreCashListType class</param>
    '''<param name="oSAMErrorCollection">An object of SAMErrorCollection class</param>

    Private Sub ValidateBaseCoreCashListType(ByVal con As SiriusConnection,
                                             ByVal oBusiness As CoreBusiness,
                                             ByRef oCashList As BaseCoreCashListType,
                                             ByRef oSAMErrorCollection As SAMErrorCollection)

        If oCashList IsNot Nothing Then
            With oCashList
                If .CashListKey = 0 AndAlso String.IsNullOrEmpty(.StatusCode) Then
                    .StatusCode = CashListStatus.Entered
                End If
                .Validate(CObj(oSAMErrorCollection))
                oSAMErrorCollection.CheckForErrors()

                'Lookup Validation

                'PMLookup-CashListType
                If Not String.IsNullOrEmpty(.TypeCode) Then
                    .TypeKey = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
                                                                        PMLookupTable.CashListType,
                                                                        .TypeCode,
                                                                        "TypeCode",
                                                                        oSAMErrorCollection)
                End If

                'PMLookup-CashListStatus
                If Not String.IsNullOrEmpty(.StatusCode) Then
                    .StatusKey = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
                                                                          PMLookupTable.CashListStatus,
                                                                          .StatusCode,
                                                                          "StatusCode",
                                                                          oSAMErrorCollection)
                End If

                'NonPMLookup-BankAccount
                If Not String.IsNullOrEmpty(.BankAccountCode) AndAlso .BankAccountKey = 0 Then
                    .BankAccountKey = GetAndValidateSpecifiedTableCode(con,
                                                                       NonPMLookupTable.BankAccount,
                                                                       NonPMLookupTableKeyFields.BankAccount,
                                                                       "Code",
                                                                       .BankAccountCode,
                                                                       oSAMErrorCollection,
                                                                       "BankAccountCode")
                End If

                'PMLookup-Currency
                If Not String.IsNullOrEmpty(.CurrencyCode.ToString) Then
                    .CurrencyKey = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
                                                                            PMLookupTable.Currency,
                                                                            .CurrencyCode.ToString,
                                                                            "CurrencyCode",
                                                                            oSAMErrorCollection)
                End If

                'PMLookup - Sub Branch 
                If Not Information.IsNothing(.SubBranchCode) AndAlso Not String.IsNullOrEmpty(.SubBranchCode.ToString) Then
                    .SubBranchID = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
                                                                            PMLookupTable.SubBranch,
                                                                            .SubBranchCode.ToString,
                                                                            "SubBranchCode",
                                                                            oSAMErrorCollection)
                End If

                'Data Validation
                If oCashList.CashListKey = 0 AndAlso oCashList.StatusCode <> CashListStatus.Entered Then
                    oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                                       "Invalid cashlist status for a newly created cashlist",
                                                       "CashListStatus")
                End If

            End With
        End If
    End Sub

    ''' <summary>
    ''' Creates a cashlist for this transaction based on the band account and currency code.
    ''' If a cashlist matching the given criteria already exists then it's id is returned so
    ''' the new cashlistitem can be appended to it's existing items.
    ''' </summary>
    '''<param name="con">An object of SiriusConnection class</param>
    '''<param name="oCashList">An object of BaseCoreCashListType class</param>
    '''<param name="oSAMErrorCollection">An object of SAMErrorCollection class</param>
    Private Sub CreateCashList(ByVal con As SiriusConnection,
                               ByRef oCashList As BaseCoreCashListType,
                               ByRef oSAMErrorCollection As SAMErrorCollection)
        If oCashList IsNot Nothing Then
            If oCashList.GetType Is GetType(BasePaymentCashListType) Then
                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Add_CashList")
                    cmd.AddInParameter("@bankaccount_code", SqlDbType.VarChar, 10).Value = Cast.NullIfDefault(oCashList.BankAccountCode, Nothing)
                    cmd.AddInParameter("@bankAccount_name", SqlDbType.VarChar, 25).Value = Cast.NullIfDefault(oCashList.BankAccountName, Nothing)
                    cmd.AddInParameter("@currency_id", SqlDbType.Int).Value = Cast.NullIfDefault(oCashList.CurrencyKey, 0)
                    cmd.AddInParameter("@cashlisttype_id", SqlDbType.Int).Value = Cast.NullIfDefault(oCashList.TypeKey, 0)
                    cmd.AddInParameter("@cashliststatus_id", SqlDbType.Int).Value = Cast.NullIfDefault(oCashList.StatusKey, 0)
                    cmd.AddInParameter("@username", SqlDbType.VarChar, 255).Value = Cast.NullIfDefault(_SiriusUser.Username, Nothing)
                    cmd.AddInParameter("@list_date", SqlDbType.DateTime).Value = Cast.NullIfDefault(oCashList.ListDate, Nothing)
                    cmd.AddInParameter("@cashlist_ref", SqlDbType.VarChar, 25).Value = Cast.NullIfDefault(oCashList.Reference, Nothing)
                    cmd.AddInParameter("@source_id", SqlDbType.Int).Value = Cast.NullIfDefault(oCashList.SourceID, 0)
                    cmd.AddOutParameter("@cashlist_id", SqlDbType.Int)
                    cmd.AddInParameter("@nbankAccount_id", SqlDbType.Int).Value = Cast.NullIfDefault(oCashList.BankAccountKey, 0)
                    cmd.AddInParameter("@subbranch_id", SqlDbType.Int).Value = Cast.NullIfDefault(oCashList.SubBranchID, 0)

                    con.ExecuteNonQuery(cmd)
                    oCashList.CashListKey = Cast.ToInt32(cmd.Parameters.Item("@cashlist_id").Value, 0)
                End Using

            ElseIf oCashList.GetType Is GetType(BaseReceiptCashListType) Then
                'Currently, we are not processing receipts in this method
                'Add invalid data error  - receipt cashList processing is not supported 

                Dim oReceiptCashList As BaseReceiptCashListType
                oReceiptCashList = DirectCast(oCashList, BaseReceiptCashListType)

                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Add_CashList")
                    cmd.AddInParameter("@bankAccount_name", SqlDbType.VarChar, 25).Value = Cast.NullIfDefault(oReceiptCashList.BankAccountName, Nothing)
                    cmd.AddInParameter("@bankaccount_code", SqlDbType.VarChar, 10).Value = Cast.NullIfDefault(oReceiptCashList.BankAccountCode, Nothing)
                    cmd.AddInParameter("@currency_id", SqlDbType.Int).Value = Cast.NullIfDefault(oReceiptCashList.CurrencyKey, 0)
                    cmd.AddInParameter("@cashlisttype_id", SqlDbType.Int).Value = Cast.NullIfDefault(oReceiptCashList.TypeKey, 0)
                    cmd.AddInParameter("@cashliststatus_id", SqlDbType.Int).Value = Cast.NullIfDefault(oReceiptCashList.StatusKey, 0)
                    cmd.AddInParameter("@username", SqlDbType.VarChar, 255).Value = Cast.NullIfDefault(_SiriusUser.Username, Nothing)
                    cmd.AddInParameter("@list_date", SqlDbType.DateTime).Value = Cast.NullIfDefault(oReceiptCashList.ListDate, Nothing)
                    cmd.AddInParameter("@cashlist_ref", SqlDbType.VarChar, 25).Value = Cast.NullIfDefault(oReceiptCashList.Reference, Nothing)
                    cmd.AddInParameter("@source_id", SqlDbType.Int).Value = Cast.NullIfDefault(oReceiptCashList.SourceID, 0)
                    cmd.AddOutParameter("@cashlist_id", SqlDbType.Int)
                    cmd.AddInParameter("@nbankAccount_id", SqlDbType.Int).Value = Cast.NullIfDefault(oReceiptCashList.BankAccountKey, 0)
                    cmd.AddInParameter("@subbranch_id", SqlDbType.Int).Value = Cast.NullIfDefault(oCashList.SubBranchID, 0)
                    con.ExecuteNonQuery(cmd)

                    oReceiptCashList.CashListKey = Cast.ToInt32(cmd.Parameters.Item("@cashlist_id").Value, 0)
                End Using

            Else
                'Unknown CashList type
                'Add invalid data error  -invalid cashlist type specified
                oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidCashListType,
                                                   "Unknown cashlist type",
                                                   "CashList")
            End If
        Else
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                               SAMInvalidData.MandatoryInputMissing.ToString,
                                               "CashList")
        End If

    End Sub

    ''' <summary>
    ''' Processes the cashlist item
    ''' </summary>
    '''<param name="con">An object of SiriusConnection class</param>
    '''<param name="oBusiness">An object of CoreBusiness class</param>
    '''<param name="oCashList">An object of BaseCoreCashListType class</param>
    '''<param name="oCashListItem">An object of BaseCoreCashListItemType class</param>
    '''<param name="oSAMErrorCollection">An object of SAMErrorCollection class.Optional parameter</param>
    Private Sub ProcessCashListItem(ByVal con As SiriusConnection,
                                    ByVal oBusiness As CoreBusiness,
                                    ByRef oCashList As BaseCoreCashListType,
                                    ByRef oCashListItem As BaseCoreCashListItemType,
                                    ByRef oSAMErrorCollection As SAMErrorCollection,
                                    Optional ByVal bIsClaimPayment As Boolean = False,
                                    Optional ByVal lClaimPaymentKey As Integer = 0,
                                    Optional ByVal bNoAuthority As Boolean = False,
                                    Optional ByVal bSettlePayment As Boolean = False,
                                    Optional ByVal bCreateCashListItem As Boolean = True,
                                    Optional ByRef bAutoAllocatePaymentSuccessful As Boolean = True)

        If oCashList.GetType Is GetType(BasePaymentCashListType) Then

            Dim oPaymentCashList As BasePaymentCashListType
            oPaymentCashList = DirectCast(oCashList, BasePaymentCashListType)

            If oCashListItem.GetType Is GetType(BasePaymentCashListItemType) Then

                Dim oPaymentCashListItem As BasePaymentCashListItemType
                oPaymentCashListItem = DirectCast(oCashListItem, BasePaymentCashListItemType)

                ValidateCashListItem(con, oBusiness, oCashList, oCashListItem, oSAMErrorCollection)
                oSAMErrorCollection.CheckForErrors()

                If Not bIsClaimPayment Then
                    con.BeginTransaction()
                End If

                Dim oStepAuthorization As New bACTCashListItem.StepAuthorization
                Dim oACTCashlistitem As New bACTCashListItem.Form

                Try
                    If bCreateCashListItem Then

                        CreateCashListItem(con, oCashListItem, oCashList, oSAMErrorCollection, bSettlePayment)
                        oSAMErrorCollection.CheckForErrors()
                    End If
                    If oPaymentCashListItem.SkipPosting = False Then
                        PostCashListItem(con, oCashListItem, oPaymentCashList.CashListKey, oSAMErrorCollection)
                        oSAMErrorCollection.CheckForErrors()
                    End If

                    Dim sOption As String
                    sOption = oBusiness.GetProductOption(ProductOption.MultiStepApproval, 1)
                    If (sOption = "1" And bSettlePayment = False) Or bNoAuthority Then

                        Dim vKeyArray(1, 5) As Object
                        vKeyArray(0, 0) = "cashlistitem_id"
                        vKeyArray(1, 0) = oCashListItem.CashListItemKey

                        vKeyArray(0, 1) = "cashlist_id"
                        vKeyArray(1, 1) = oCashList.CashListKey

                        vKeyArray(0, 2) = "cashlisttype_id"
                        vKeyArray(1, 2) = oCashList.TypeKey

                        vKeyArray(0, 3) = "actionkey"
                        vKeyArray(1, 3) = "approve"

                        vKeyArray(0, 4) = "cash_list_item_mode"
                        vKeyArray(1, 4) = 2

                        vKeyArray(0, 5) = "payment_id"
                        vKeyArray(1, 5) = lClaimPaymentKey

                        Dim sUserGroupCode As String
                        Dim icomReturnValue As Integer
                        Dim sTaskDescComplete As String
                        Dim sTaskDesc As String
                        Dim lPMWrkTaskInstanceCnt As Integer

                        SAMFunc.InitialiseSBOObject(con, oStepAuthorization, _SiriusUser, "bACTCashlistitem.StepAuthorization")
                        SAMFunc.InitialiseSBOObject(con, oACTCashlistitem, _SiriusUser, "bACTCashlistitem.Form")
                        If oCashList.TypeCode = "CP" Then
                            oStepAuthorization.PaymentType = 1
                        Else
                            oStepAuthorization.PaymentType = 2
                        End If
                        oStepAuthorization.CashListSourceID = oCashList.SourceID
                        icomReturnValue = oStepAuthorization.GetStepGroupCode(r_sGroupCode:=sUserGroupCode, r_sErrorMessage:="", IsViaBulkClaimPayment:=oCashListItem.IsViaBulkClaimPayment)
                        If icomReturnValue <> PMEReturnCode.PMTrue AndAlso icomReturnValue <> PMEReturnCode.PMNotFound Then
                            RaiseComMethodException("bACTCashlistitem.StepAuthorization.GetStepGroupCode", icomReturnValue)
                        End If

                        icomReturnValue = oACTCashlistitem.GetCashListType(v_lCashListTypeID:=oCashList.TypeKey,
                                         r_sCashListType:=sTaskDesc)
                        If icomReturnValue <> PMEReturnCode.PMTrue AndAlso icomReturnValue <> PMEReturnCode.PMNotFound Then
                            RaiseComMethodException("bACTCashlistitem.StepAuthorization.GetStepGroupCode", icomReturnValue)
                        End If

                        sTaskDescComplete = sTaskDesc & " - Cash / Cheque" & Space(1)
                        sTaskDescComplete = sTaskDescComplete & " - Reference: " & Trim$(oCashListItem.OurReference) & Space(1)
                        'sTaskDesc = sTaskDesc & "List Date :" & Trim$(vListDate)
                        sTaskDescComplete = sTaskDescComplete & " - The Amount: " & Format(oCashListItem.Amount, "#,##0.00")

                        icomReturnValue = oACTCashlistitem.AddTaskToWorkManager(r_lPMWrkTaskInstanceCnt:=lPMWrkTaskInstanceCnt, v_sCustomer:=oCashListItem.AccountShortCode, v_sDescription:=sTaskDescComplete, v_dtTaskDueDate:=DateAdd("s", -1, DateAdd("d", 1, Date.Today)), v_sTaskCode:="SHOWCLITEM", v_sTaskGroupCode:="SLACS", v_sUserGroupCode:=sUserGroupCode, v_vKeyArray:=vKeyArray)
                        If icomReturnValue <> PMEReturnCode.PMTrue AndAlso icomReturnValue <> PMEReturnCode.PMNotFound Then
                            RaiseComMethodException("bACTCashlistitem.Form.AddTaskToWorkManager", icomReturnValue)
                        End If

                        If lClaimPaymentKey <> 0 Then
                            icomReturnValue = oACTCashlistitem.AddCashListItemClaimLink(v_lClaim_payment_Id:=lClaimPaymentKey,
                                                                         v_lClaim_receipt_id:=0,
                                                                         v_lCashListItem_id:=oCashListItem.CashListItemKey)
                            If icomReturnValue <> PMEReturnCode.PMTrue AndAlso icomReturnValue <> PMEReturnCode.PMNotFound Then
                                RaiseComMethodException("bACTCashlistitem.AddCashListItemClaimLink", icomReturnValue)
                            End If
                        End If

                    ElseIf oCashListItem.AllocationDetails IsNot Nothing AndAlso oCashListItem.AllocationDetails.AutoAllocate Then
                        Try
                            AllocateCashListItem(oBusiness, con, oPaymentCashList, oPaymentCashListItem)
                            oSAMErrorCollection.CheckForErrors()
                        Catch ex As Exception
                            bAutoAllocatePaymentSuccessful = False
                        End Try
                    End If

                    If Not bIsClaimPayment Then
                        con.CommitTransaction()
                    End If
                Catch ex As Exception
                    If Not bIsClaimPayment Then
                        con.RollbackTransaction()
                    End If
                    Throw
                Finally
                    If oStepAuthorization IsNot Nothing Then
                        oStepAuthorization.Dispose()
                        oStepAuthorization = Nothing
                    End If
                    If oACTCashlistitem IsNot Nothing Then
                        oACTCashlistitem.Dispose()
                        oACTCashlistitem = Nothing
                    End If
                End Try

            ElseIf oCashListItem.GetType Is GetType(BaseReceiptCashListItemType) Then
                'Currently, we are not processing receipts in this method
                'Add invalid data error  - payment cash list can not contain reciept cash list item 
                oSAMErrorCollection.AddInvalidData(SAMInvalidData.ReceiptCashListItemProcessingIsNotSupported,
                                                  "Cash list of type Payment can not contain cash list item of type Receipt",
                                                   "CashListItem")

            Else
                'Unknown CashList item type
                'Add invalid data error  -invalid cashlist item type specified
                oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidCashListItemType,
                                                   "Unknown cashlist item type",
                                                   "CashListItem")
            End If

        ElseIf oCashList.GetType Is GetType(BaseReceiptCashListType) Then
            Dim oReceiptCashList As BaseReceiptCashListType
            oReceiptCashList = DirectCast(oCashList, BaseReceiptCashListType)


            If oCashListItem.GetType Is GetType(BaseReceiptCashListItemType) Then

                Dim oReceiptCashListItem As BaseReceiptCashListItemType
                oReceiptCashListItem = DirectCast(oCashListItem, BaseReceiptCashListItemType)

                ValidateCashListItem(con, oBusiness, oCashList, oCashListItem, oSAMErrorCollection)
                oSAMErrorCollection.CheckForErrors()

                If (oCashListItem.CashListItemKey <> 0) Then
                    'Currently, this application supports creation of new cash list item, not modification of existing cash list item.
                    'If oCashListItem.CashListItemKey is not zero then raise error
                    'Add invalid data error  - cashlist updation is not supported
                    oSAMErrorCollection.AddInvalidData(SAMInvalidData.CashListItemUpdationIsNotSupported,
                                                       "Cashlist item updation is not supported",
                                                       "CashListItem")
                Else
                    Dim oACTCashlistitem As New bACTCashListItem.Form

                    Try
                        CreateCashListItem(con, oCashListItem, oCashList, oSAMErrorCollection)
                        oSAMErrorCollection.CheckForErrors()

                        PostCashListItem(con, oCashListItem, oReceiptCashList.CashListKey, oSAMErrorCollection)
                        oSAMErrorCollection.CheckForErrors()

                        If oCashListItem.AllocationDetails.AutoAllocate Then
                            Try
                                If oReceiptCashListItem.InstalmentPlanDetails Is Nothing Then
                                    AllocateCashListItem(oBusiness, con, oReceiptCashList, oReceiptCashListItem)
                                End If
                                 oSAMErrorCollection.CheckForErrors()
                            Catch ex As Exception
                                bAutoAllocatePaymentSuccessful = False
                            End Try
                        End If

                    Catch ex As Exception

                        Throw

                    End Try
                End If
            Else
                'Unknown CashList item type
                'Add invalid data error  -invalid cashlist item type specified
                oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidCashListItemType,
                                                   "Unknown cashlist item type",
                                                   "CashListItem")
            End If

        Else
            'Unknown CashList type
            'Add invalid data error  -invalid cashlist type specified
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidCashListType,
                                               "Unknown cashlist type",
                                               "CashList")
        End If
    End Sub

    ''' <summary>
    ''' Validates the claim payment cashlist item
    ''' </summary>
    '''<param name="con">An object of SiriusConnection class</param>
    '''<param name="oBusiness">An object of CoreBusiness class</param>
    '''<param name="oCashList">An object of BaseCoreCashListType class</param>
    '''<param name="oCashListItem">An object of BaseCoreCashListItemType class</param>
    '''<param name="oSAMErrorCollection">An object of SAMErrorCollection class</param>
    Private Sub ValidateCashListItem(ByVal con As SiriusConnection,
                                     ByVal oBusiness As CoreBusiness,
                                     ByRef oCashList As BaseCoreCashListType,
                                     ByRef oCashListItem As BaseCoreCashListItemType,
                                     ByRef oSAMErrorCollection As SAMErrorCollection)

        If oCashList.GetType Is GetType(BasePaymentCashListType) Then

            Dim oPaymentCashList As BasePaymentCashListType
            oPaymentCashList = DirectCast(oCashList, BasePaymentCashListType)

            If oCashListItem.GetType Is GetType(BasePaymentCashListItemType) Then
                Dim sOption As String
                sOption = oBusiness.GetProductOption(ProductOption.MultiStepApproval, 1)
                If sOption = "1" Then
                    Dim oStepAuthorization As New bACTCashListItem.StepAuthorization
                    Dim sUserGroupCode As String = ""
                    Dim icomReturnValue As Integer
                    Dim sErrorMessage As String = String.Empty
                    Try
                        SAMFunc.InitialiseSBOObject(con, oStepAuthorization, _SiriusUser, "bACTCashlistitem.StepAuthorization")
                        If oCashList.TypeCode = "CP" Then
                            oStepAuthorization.PaymentType = 1
                        Else
                            oStepAuthorization.PaymentType = 2
                        End If

                        oStepAuthorization.CashListSourceID = oCashList.SourceID
                        icomReturnValue = oStepAuthorization.GetStepGroupCode(r_sGroupCode:=sUserGroupCode, r_sErrorMessage:=sErrorMessage)

                        If sErrorMessage <> "" Then
                            oSAMErrorCollection.AddInvalidData(SAMInvalidData.DebtorUserGroupsAreNotSetup,
                                                                SAMInvalidData.DebtorUserGroupsAreNotSetup.ToString,
                                                               sErrorMessage)
                            oSAMErrorCollection.CheckForErrors()
                        ElseIf icomReturnValue <> PMEReturnCode.PMTrue AndAlso icomReturnValue <> PMEReturnCode.PMNotFound Then
                            RaiseComMethodException("bACTCashlistitem.StepAuthorization.GetStepGroupCode", icomReturnValue)
                        End If

                    Catch ex As Exception
                        Throw
                    Finally
                        If oStepAuthorization IsNot Nothing Then
                            oStepAuthorization.Dispose()
                            oStepAuthorization = Nothing
                        End If

                    End Try

                End If
                Dim oPaymentCashListItem As BasePaymentCashListItemType
                oPaymentCashListItem = DirectCast(oCashListItem, BasePaymentCashListItemType)

                ValidateBasePaymentCashListItemType(oBusiness, con, oPaymentCashList, oPaymentCashListItem, oSAMErrorCollection, sOption)
                oSAMErrorCollection.CheckForErrors()

            ElseIf oCashListItem.GetType Is GetType(BaseReceiptCashListItemType) Then
                'Currently, we are not processing receipts in this method
                'Add invalid data error  - payment cash list can not contain reciept cash list item 
                oSAMErrorCollection.AddInvalidData(SAMInvalidData.ReceiptCashListItemProcessingIsNotSupported,
                                                  "Cash list of type Payment can not contain cash list item of type Receipt",
                                                   "CashListItem")

            Else
                'Unknown CashListItem type
                'Add invalid data error  -invalid cashlist item type specified
                oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidCashListItemType,
                                                   "Unknown cashlist item type",
                                                   "CashListItem")
            End If

        ElseIf oCashList.GetType Is GetType(BaseReceiptCashListType) Then
            Dim oReceiptCashList As BaseReceiptCashListType
            oReceiptCashList = DirectCast(oCashList, BaseReceiptCashListType)

            If oCashListItem.GetType Is GetType(BaseReceiptCashListItemType) Then

                Dim oReceiptCashListItem As BaseReceiptCashListItemType
                oReceiptCashListItem = DirectCast(oCashListItem, BaseReceiptCashListItemType)
                ValidateBaseReceiptCashListItemType(oBusiness, con, oReceiptCashList, oReceiptCashListItem, oSAMErrorCollection)
                oSAMErrorCollection.CheckForErrors()
            Else

                'Unknown CashListItem type
                'Add invalid data error  -invalid cashlist item type specified
                oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidCashListItemType,
                                                   "Unknown cashlist item type",
                                                   "CashListItem")
            End If
        Else
            'Unknown CashList type
            'Add invalid data error  -invalid cashlist type specified
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidCashListType,
                                               "Unknown cashlist type",
                                               "CashList")
        End If

    End Sub

    ''' <summary>
    ''' Processes the core cashlist item
    ''' </summary>
    '''<param name="con">An object of SiriusConnection class</param>
    '''<param name="oBusiness">An object of CoreBusiness class</param>
    '''<param name="oCashList">An object of BaseCoreCashListType class</param>
    '''<param name="oCashListItem">An object of BaseCoreCashListItemType class</param>
    '''<param name="oSAMErrorCollection">An object of SAMErrorCollection) class</param>
    Private Sub ValidateBaseCoreCashListItemType(ByVal con As SiriusConnection,
                                                 ByVal oBusiness As CoreBusiness,
                                                 ByRef oCashList As BaseCoreCashListType,
                                                 ByRef oCashListItem As BaseCoreCashListItemType,
                                                 ByRef oSAMErrorCollection As SAMErrorCollection,
                                                 Optional ByVal sResultMultiStepApproval As String = "0")
        If oCashList.GetType Is GetType(BasePaymentCashListType) Then

            Dim oPaymentCashList As BasePaymentCashListType
            oPaymentCashList = DirectCast(oCashList, BasePaymentCashListType)

            If oCashListItem.GetType Is GetType(BasePaymentCashListItemType) Then

                Dim oPaymentCashListItem As BasePaymentCashListItemType
                oPaymentCashListItem = DirectCast(oCashListItem, BasePaymentCashListItemType)

                If oPaymentCashListItem.CashListItemKey = 0 Then
                    If String.IsNullOrEmpty(oPaymentCashListItem.StatusCode) Then
                        oPaymentCashListItem.StatusCode = CashListItemPaymentStatus.Issued
                    End If
                    If String.IsNullOrEmpty(oPaymentCashListItem.AllocationStatusCode) Then
                        oPaymentCashListItem.AllocationStatusCode = AllocationStatus.Unallocated
                    End If
                End If

                oCashListItem.Validate(CObj(oSAMErrorCollection))
                oSAMErrorCollection.CheckForErrors()

                If oPaymentCashListItem IsNot Nothing Then
                    With oPaymentCashListItem
                        'Lookup Validation

                        'PMLookup-MediaType
                        If Not String.IsNullOrEmpty(.MediaTypeCode) Then
                            .MediaTypeKey = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
                                                                                     PMLookupTable.MediaType,
                                                                                     .MediaTypeCode,
                                                                                     "MediaTypeCode",
                                                                                     oSAMErrorCollection)
                        End If

                        'NonPMLookup-Account
                        If Not String.IsNullOrEmpty(.AccountShortCode) Then
                            .AccountKey = GetAndValidateSpecifiedTableCode(con,
                                                                           NonPMLookupTable.Account,
                                                                           NonPMLookupTableKeyFields.Account,
                                                                           "Short_Code",
                                                                           .AccountShortCode,
                                                                           oSAMErrorCollection,
                                                                           "AccountShortCode")
                        End If

                        'PMLookup-AllocationStatus
                        If Not String.IsNullOrEmpty(.AllocationStatusCode) Then
                            .AllocationStatusKey = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
                                                                                            PMLookupTable.AllocationStatus,
                                                                                            .AllocationStatusCode,
                                                                                            "AllocationStatusCode",
                                                                                            oSAMErrorCollection)
                        End If

                        'PMLookup-Country
                        If .ContactAddress IsNot Nothing Then
                            If Not String.IsNullOrEmpty(.ContactAddress.CountryCode) Then
                                .ContactAddress.CountryKey = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
                                                                                                      PMLookupTable.Country,
                                                                                                      .ContactAddress.CountryCode,
                                                                                                      "CountryCode",
                                                                                                      oSAMErrorCollection)
                            End If
                        End If

                        'Data Validation
                        If .CashListItemKey = 0 Then 'New cashlistitem
                            'For new cashlist item (CashListItemKey=0), StatusCode should be ISS(issued)
                            If .StatusCode <> CashListItemPaymentStatus.Issued AndAlso (sResultMultiStepApproval = "1" AndAlso .StatusCode = CashListItemPaymentStatus.PendingApproval) = False Then
                                oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                                                   "Invalid payment status code given for new cashlist item",
                                                                   "StatusCode",
                                                                   .StatusCode)
                            End If

                            'For new cashlist item (CashListItemKey=0), AllocationStatusCode should be U(Unallocated)
                            If .AllocationStatusCode <> AllocationStatus.Unallocated Then
                                oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                                                   "Invalid allocation status code given for new cashlist item",
                                                                   "AllocationStatusCode",
                                                                   .AllocationStatusCode)
                            End If
                        End If

                        'Media reference mandatory checking

                        Dim dtMedia As New DataTable

                        ' Get media type details
                        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_MediaType_By_ID")

                            cmd.AddInParameter("@MediaType_ID", SqlDbType.Int).Value = .MediaTypeKey

                            con.ExecuteDataTable(cmd, dtMedia)
                        End Using

                        ' If there are MediaType records in the database for the given media type id
                        If dtMedia IsNot Nothing AndAlso dtMedia.Rows.Count > 0 Then
                            Dim drMedia As DataRow = dtMedia.Rows(0)
                            If Cast.ToInt32(drMedia.Item("is_media_reference_mandatory"), 0) = 1 Then
                                'If media reference is mandatory in the database, the payment item must contain a valid media reference
                                If String.IsNullOrEmpty(.MediaReference) Then
                                    oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                                                       SAMInvalidData.MandatoryInputMissing.ToString,
                                                                       "MediaReference")
                                Else
                                    'Check media reference is valid
                                    Dim iValidMediaReference As Integer
                                    Dim iValidate_UI As Integer
                                    Dim bInvalid As Boolean
                                    ' Get media reference details
                                    Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_ACT_Do_CashListItem_Validate")
                                        cmd.AddInParameter("@cashlist_id", SqlDbType.Int).Value = oPaymentCashList.CashListKey
                                        cmd.AddInParameter("@cashlistitem_id", SqlDbType.Int).Value = .CashListItemKey
                                        cmd.AddInParameter("@mediatype_id", SqlDbType.Int).Value = .MediaTypeKey
                                        cmd.AddInParameter("@media_ref", SqlDbType.VarChar, 100).Value = .MediaReference
                                        cmd.AddOutParameter("@period_months", SqlDbType.TinyInt)
                                        cmd.AddOutParameter("@valid", SqlDbType.TinyInt)
                                        cmd.AddOutParameter("@validate_ui", SqlDbType.TinyInt)

                                        con.ExecuteNonQuery(cmd)

                                        iValidMediaReference = Cast.ToInt16(cmd.Parameters.Item("@valid").Value, 0)
                                        iValidate_UI = Cast.ToInt16(cmd.Parameters.Item("@validate_ui").Value, 0)
                                    End Using
                                    If iValidMediaReference = 1 Then
                                        If iValidate_UI = 1 Then
                                            'Check the media reference is unique across the current CashList
                                            If (oPaymentCashList.PaymentItem.Length > 1) Then
                                                For Each oItem As BasePaymentCashListItemType In oPaymentCashList.PaymentItem
                                                    If Not oPaymentCashListItem.Equals(oItem) Then
                                                        If oItem.MediaReference = oPaymentCashListItem.MediaReference Then
                                                            bInvalid = True
                                                            Exit For
                                                        End If
                                                    End If
                                                Next
                                            End If
                                        End If
                                    Else 'MediaReference has been already used in database table
                                        bInvalid = True
                                    End If
                                    If bInvalid Then
                                        oSAMErrorCollection.AddInvalidData(SAMInvalidData.MediaReferenceAlreadyUsed,
                                                               "MediaReference has already been used",
                                                               "MediaReference",
                                                               .MediaReference)
                                    End If

                                End If
                            End If
                        End If

                        'Check the party holding the account is deleted

                        Dim dtParty As New DataTable

                        ' Get Party key
                        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_ACT_Get_PartyCnt_From_AccountID")

                            cmd.AddInParameter("@accountid", SqlDbType.Int).Value = .AccountKey

                            con.ExecuteDataTable(cmd, dtParty)
                        End Using

                        ' If there is a pary key returned
                        If dtParty IsNot Nothing AndAlso dtParty.Rows.Count > 0 Then
                            Dim drParty As DataRow = dtParty.Rows(0)
                            .PartyKey = Cast.ToInt32(drParty.Item("account_key"), 0)
                            Dim o_lAccountID As Integer
                            Dim o_sAccountCode As String = String.Empty
                            Dim o_sAccountShortCode As String = String.Empty
                            Dim o_bIsDeleted As Boolean

                            GetAccountDetailsForParty(con,
                                                      .PartyKey,
                                                      o_lAccountID,
                                                      o_sAccountCode,
                                                      o_sAccountShortCode,
                                                      o_bIsDeleted)

                            If (o_bIsDeleted) Then
                                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMBusinessWarnings.PaymentToDeletedParty,
                                                                   "This payment is being made to a party that has been deleted in Back Office",
                                                                   "PartyKey")
                            End If

                        End If

                        'Check whether the bank account id and account id of payment item are same or not 

                        Dim dtAccount As New DataTable

                        ' Get bank account details
                        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_ACT_Select_BankAccount")

                            cmd.AddInParameter("@bankaccount_id", SqlDbType.Int).Value = oPaymentCashList.BankAccountKey

                            con.ExecuteDataTable(cmd, dtAccount)
                        End Using

                        ' If there are bank account details
                        If dtAccount IsNot Nothing AndAlso dtAccount.Rows.Count > 0 Then
                            Dim drAccount As DataRow = dtAccount.Rows(0)
                            If (Cast.ToInt32(drAccount.Item("account_id"), 0) = .AccountKey) Then
                                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.PaymentAccountAndBankAccountAreSame,
                                                                   "Account chosen is the same as the bank account",
                                                                   "AccountKey")
                            End If
                        End If
                    End With
                End If
            ElseIf oCashListItem.GetType Is GetType(BaseReceiptCashListItemType) Then
                'Currently, we are not processing receipts in this method
                'Add invalid data error  - payment cash list can not contain reciept cash list item 
                oSAMErrorCollection.AddInvalidData(SAMInvalidData.ReceiptCashListItemProcessingIsNotSupported,
                                                  "Cash list of type Payment can not contain cash list item of type Receipt",
                                                   "CashListItem")

            Else
                'Unknown CashListItem type
                'Add invalid data error  -invalid cashlist item type specified
                oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidCashListItemType,
                                                   "Unknown cashlist item type",
                                                   "CashListItem")
            End If

        ElseIf oCashList.GetType Is GetType(BaseReceiptCashListType) Then
            'Currently, we are not processing receipts in this method
            'Add invalid data error  - receipt cashList processing is not supported 

            'oSAMErrorCollection.AddInvalidData(SAMInvalidData.ReceiptCashListProcessingIsNotSupported, _
            '                                   "Receipt handling is not supported", _
            '                                   "CashList")
            Dim oReceiptCashList As BaseReceiptCashListType
            oReceiptCashList = DirectCast(oCashList, BaseReceiptCashListType)

            If oCashListItem.GetType Is GetType(BaseReceiptCashListItemType) Then

                Dim oReceiptCashListItem As BaseReceiptCashListItemType
                oReceiptCashListItem = DirectCast(oCashListItem, BaseReceiptCashListItemType)
                If oReceiptCashListItem IsNot Nothing Then
                    With oReceiptCashListItem
                        'Lookup Validation

                        'PMLookup-MediaType
                        If Not String.IsNullOrEmpty(.MediaTypeCode) Then
                            .MediaTypeKey = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
                                                                                     PMLookupTable.MediaType,
                                                                                     .MediaTypeCode,
                                                                                     "MediaTypeCode",
                                                                                     oSAMErrorCollection)
                        End If

                        'NonPMLookup-Account
                        If Not String.IsNullOrEmpty(.AccountShortCode) Then
                            .AccountKey = GetAndValidateSpecifiedTableCode(con,
                                                                           NonPMLookupTable.Account,
                                                                           NonPMLookupTableKeyFields.Account,
                                                                           "Short_Code",
                                                                           .AccountShortCode,
                                                                           oSAMErrorCollection,
                                                                           "AccountShortCode")
                        End If

                        'PMLookup-AllocationStatus
                        If Not String.IsNullOrEmpty(.AllocationStatusCode) Then
                            .AllocationStatusKey = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
                                                                                            PMLookupTable.AllocationStatus,
                                                                                            .AllocationStatusCode,
                                                                                            "AllocationStatusCode",
                                                                                            oSAMErrorCollection)
                        End If

                        'PMLookup-Country
                        If .ContactAddress IsNot Nothing Then
                            If Not String.IsNullOrEmpty(.ContactAddress.CountryCode) Then
                                .ContactAddress.CountryKey = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
                                                                                                      PMLookupTable.Country,
                                                                                                      .ContactAddress.CountryCode,
                                                                                                      "CountryCode",
                                                                                                      oSAMErrorCollection)
                            End If

                        End If

                    End With
                End If
            End If

        Else
            'Unknown CashList type
            'Add invalid data error  -invalid cashlist type specified
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidCashListType,
                                               "Unknown cashlist type",
                                               "CashList")
        End If
    End Sub

    Private Sub CreateCashListItem(ByVal con As SiriusConnection,
                               ByRef oCashListItem As BaseCoreCashListItemType,
                               ByRef oCashList As BaseCoreCashListType,
                               ByRef oSAMErrorCollection As SAMErrorCollection)

        CreateCashListItem(con, oCashListItem, oCashList, oSAMErrorCollection, False)
    End Sub

    ''' <summary>
    ''' Creates a cashlist item for this payment
    ''' </summary>
    '''<param name="con">An object of SiriusConnection class</param>
    '''<param name="oCashListItem">An object of BaseCoreCashListItemType class</param>
    '''<param name="oCashList">An object of BaseCoreCashListType class</param>
    '''<param name="oSAMErrorCollection">An object of SAMErrorCollection class</param>
    Private Sub CreateCashListItem(ByVal con As SiriusConnection,
                                   ByRef oCashListItem As BaseCoreCashListItemType,
                                   ByRef oCashList As BaseCoreCashListType,
                                   ByRef oSAMErrorCollection As SAMErrorCollection,
                                   ByVal bSettlePayment As Boolean)

        If oCashList IsNot Nothing AndAlso oCashListItem IsNot Nothing Then

            If oCashList.GetType Is GetType(BasePaymentCashListType) Then

                Dim oPaymentCashList As BasePaymentCashListType
                oPaymentCashList = DirectCast(oCashList, BasePaymentCashListType)

                If oCashListItem.GetType Is GetType(BasePaymentCashListItemType) Then

                    Dim oPaymentCashListItem As BasePaymentCashListItemType
                    oPaymentCashListItem = DirectCast(oCashListItem, BasePaymentCashListItemType)

                    Dim ibatchid As Integer = 0
                    Dim ibatchidresult As Integer = 0

                    If Not String.IsNullOrEmpty(oPaymentCashListItem.BankReference) Then
                        Using cmdbatch As SiriusCommand = SiriusCommand.FromProcedure("spu_ACT_Select_Batch_FromBatchRef")
                            cmdbatch.AddOutParameter("@batch_id", SqlDbType.Int)
                            cmdbatch.AddInParameter("@batch_ref", SqlDbType.VarChar, 25).Value = oPaymentCashListItem.BankReference
                            con.ExecuteNonQuery(cmdbatch)
                            ibatchid = Cast.ToInt32(cmdbatch.Parameters.Item("@batch_id").Value, 0)
                        End Using
                        If ibatchid = 0 Then

                            Using cmdaddbatch As SiriusCommand = SiriusCommand.FromProcedure("spu_ACT_Add_Batch")
                                cmdaddbatch.AddOutParameter("@batch_id", SqlDbType.Int)
                                cmdaddbatch.AddInParameter("@company_id", SqlDbType.SmallInt).Value = 1
                                cmdaddbatch.AddInParameter("@batchstatus_id", SqlDbType.SmallInt).Value = 1
                                cmdaddbatch.AddInParameter("@user_id", SqlDbType.SmallInt).Value = 1
                                cmdaddbatch.AddInParameter("@batch_ref", SqlDbType.VarChar, 25).Value = oPaymentCashListItem.BankReference
                                cmdaddbatch.AddInParameter("@created_date", SqlDbType.DateTime).Value = DateTime.Now()
                                cmdaddbatch.AddInParameter("@authorised_date", SqlDbType.DateTime).Value = SqlDateTime.Null
                                cmdaddbatch.AddInParameter("@accounting_date", SqlDbType.DateTime).Value = SqlDateTime.Null
                                cmdaddbatch.AddInParameter("@comment", SqlDbType.VarChar, 60).Value = SqlString.Null
                                cmdaddbatch.AddInParameter("@batch_type_id", SqlDbType.Int).Value = 3
                                cmdaddbatch.AddInParameter("@batch_source_id", SqlDbType.Int).Value = SqlInt32.Null
                                cmdaddbatch.AddInParameter("@xml_object", SqlDbType.VarChar, 1000).Value = SqlString.Null
                                cmdaddbatch.AddInParameter("@exportdate", SqlDbType.DateTime).Value = SqlDateTime.Null
                                cmdaddbatch.AddInParameter("@reexportdate", SqlDbType.DateTime).Value = SqlDateTime.Null
                                cmdaddbatch.AddInParameter("@mediatypeid", SqlDbType.Int).Value = SqlInt32.Null
                                cmdaddbatch.AddInParameter("@totalamount", SqlDbType.Decimal, 9, 4).Value = SqlDecimal.Null
                                cmdaddbatch.AddInParameter("@totaltransactions", SqlDbType.Int).Value = SqlInt32.Null
                                cmdaddbatch.AddInParameter("@importeddate", SqlDbType.DateTime).Value = SqlDateTime.Null
                                cmdaddbatch.AddInParameter("@rejectamount", SqlDbType.Decimal, 9, 4).Value = SqlDecimal.Null
                                cmdaddbatch.AddInParameter("@rejecttransactions", SqlDbType.Int).Value = SqlInt32.Null
                                cmdaddbatch.AddInParameter("@closeddate", SqlDbType.DateTime).Value = SqlDateTime.Null
                                cmdaddbatch.AddInParameter("@interfacecode", SqlDbType.VarChar, 30).Value = SqlString.Null
                                cmdaddbatch.AddInParameter("@autoclose", SqlDbType.TinyInt).Value = 0

                                con.ExecuteNonQuery(cmdaddbatch)
                                ibatchidresult = Cast.ToInt32(cmdaddbatch.Parameters.Item("@batch_id").Value, 0)
                            End Using
                        Else
                            ibatchidresult = ibatchid
                        End If
                    End If

                    Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Add_CashListItem_Payment")

                        If String.IsNullOrEmpty(oPaymentCashListItem.TaxBandCode) = False Then
                            oPaymentCashListItem.TaxBandKey = GetAndValidateSpecifiedTableCode(con,
                                        PMLookupTable.TaxBand,
                                        "Tax_Band_Id",
                                        "code",
                                        oPaymentCashListItem.TaxBandCode,
                                        oSAMErrorCollection, "TaxBandCode")
                        End If
                        'Order of parameters has been rearranged 
                        cmd.AddInOutParameter("@cashlistitem_id", SqlDbType.Int).Value = Cast.NullIfDefault(oPaymentCashListItem.CashListItemKey, 0)

                        If (oPaymentCashListItem.IsProduceDocument = True) Then
                            cmd.AddInParameter("@letter", SqlDbType.TinyInt).Value = 1
                        Else
                            cmd.AddInParameter("@letter", SqlDbType.TinyInt).Value = 0
                        End If
                        cmd.AddInParameter("@batch_id", SqlDbType.Int).Value = Cast.NullIfDefault(ibatchidresult, 0)
                        cmd.AddInParameter("@cashlist_id", SqlDbType.Int).Value = Cast.NullIfDefault(oPaymentCashList.CashListKey, 0)
                        cmd.AddInParameter("@account_code", SqlDbType.VarChar, 30).Value = Cast.NullIfDefault(oPaymentCashListItem.AccountShortCode, Nothing)
                        cmd.AddInParameter("@transaction_date", SqlDbType.DateTime).Value = Cast.NullIfDefault(oPaymentCashListItem.TransactionDate, DBNull.Value)
                        If oPaymentCashListItem.Collection_Date > DateTime.MinValue Then
                            cmd.AddInParameter("@Collection_Date", SqlDbType.DateTime).Value = Cast.NullIfDefault(oPaymentCashListItem.Collection_Date, DBNull.Value) 'DateTime.MinValue)
                        End If
                        cmd.AddInParameter("@dcheque_date", SqlDbType.DateTime).Value = Cast.NullIfDefault(oPaymentCashListItem.ChequeDate, DateTime.MinValue)
                        cmd.AddInParameter("@amount", SqlDbType.Money).Value = Cast.NullIfDefault(If(bSettlePayment, oPaymentCashListItem.Amount * -1, oPaymentCashListItem.Amount), 0.0)
                        cmd.AddInParameter("@amount_tendered", SqlDbType.Money).Value = Cast.NullIfDefault((oPaymentCashListItem.Amount_Tendered), 0.0)
                        cmd.AddInParameter("@original_amount", SqlDbType.Money).Value = oPaymentCashListItem.Original_Amount
                        cmd.AddInParameter("@media_ref", SqlDbType.VarChar, 100).Value = Cast.NullIfDefault(oPaymentCashListItem.MediaReference, Nothing)
                        cmd.AddInParameter("@our_ref", SqlDbType.VarChar, 255).Value = Cast.NullIfDefault(oPaymentCashListItem.OurReference, Nothing)
                        cmd.AddInParameter("@their_ref", SqlDbType.VarChar, 30).Value = Cast.NullIfDefault(oPaymentCashListItem.TheirReference, Nothing)
                        cmd.AddInParameter("@contact_name", SqlDbType.VarChar, 255).Value = Cast.NullIfDefault(oPaymentCashListItem.ContactName, Nothing)

                        cmd.AddInParameter("@payment_type_id", SqlDbType.Int).Value = Cast.NullIfDefault(oPaymentCashListItem.TypeKey, 0)
                        cmd.AddInParameter("@payment_status_id", SqlDbType.Int).Value = Cast.NullIfDefault(oPaymentCashListItem.StatusKey, 0)

                        cmd.AddInParameter("@allocationstatus_id", SqlDbType.Int).Value = Cast.NullIfDefault(oPaymentCashListItem.AllocationStatusKey, 0)
                        cmd.AddInParameter("@mediatype_id", SqlDbType.Int).Value = Cast.NullIfDefault(oPaymentCashListItem.MediaTypeKey, 0)
                        cmd.AddInParameter("@pmuser_id", SqlDbType.Int).Value = _SiriusUser.UserID
                        cmd.AddInParameter("@receipt_details", SqlDbType.VarChar, 500).Value = Cast.NullIfDefault(oPaymentCashListItem.FurtherDetails, Nothing)

                        cmd.AddInParameter("@ntax_band_id", SqlDbType.Int).Value = Cast.NullIfDefault(oPaymentCashListItem.TaxBandKey, 0)

                        'Contact address
                        If oPaymentCashListItem.ContactAddress IsNot Nothing Then
                            cmd.AddInParameter("@address1", SqlDbType.VarChar, 60).Value = Cast.NullIfDefault(oPaymentCashListItem.ContactAddress.AddressLine1, Nothing)
                            cmd.AddInParameter("@address2", SqlDbType.VarChar, 60).Value = Cast.NullIfDefault(oPaymentCashListItem.ContactAddress.AddressLine2, Nothing)
                            cmd.AddInParameter("@address3", SqlDbType.VarChar, 60).Value = Cast.NullIfDefault(oPaymentCashListItem.ContactAddress.AddressLine3, Nothing)
                            cmd.AddInParameter("@address4", SqlDbType.VarChar, 60).Value = Cast.NullIfDefault(oPaymentCashListItem.ContactAddress.AddressLine4, Nothing)
                            cmd.AddInParameter("@postal_code", SqlDbType.VarChar, 20).Value = Cast.NullIfDefault(oPaymentCashListItem.ContactAddress.PostCode, Nothing)
                            cmd.AddInParameter("@address_country_id", SqlDbType.Int).Value = Cast.NullIfDefault(oPaymentCashListItem.ContactAddress.CountryKey, 0)
                        Else
                            cmd.AddInParameter("@address1", SqlDbType.VarChar, 60).Value = SqlString.Null
                            cmd.AddInParameter("@address2", SqlDbType.VarChar, 60).Value = SqlString.Null
                            cmd.AddInParameter("@address3", SqlDbType.VarChar, 60).Value = SqlString.Null
                            cmd.AddInParameter("@address4", SqlDbType.VarChar, 60).Value = SqlString.Null
                            cmd.AddInParameter("@postal_code", SqlDbType.VarChar, 20).Value = SqlString.Null
                            cmd.AddInParameter("@address_country_id", SqlDbType.Int).Value = SqlInt16.Null
                        End If

                        'Credit card details
                        If oPaymentCashListItem.CreditCard IsNot Nothing Then
                            cmd.AddInParameter("@cc_name", SqlDbType.VarChar, 50).Value = Cast.NullIfDefault(oPaymentCashListItem.CreditCard.NameOnCreditCard, Nothing)
                            cmd.AddInParameter("@cc_number", SqlDbType.VarChar, 30).Value = Cast.NullIfDefault(oPaymentCashListItem.CreditCard.Number, Nothing)
                            cmd.AddInParameter("@cc_expiry_date", SqlDbType.VarChar, 10).Value = Cast.NullIfDefault(oPaymentCashListItem.CreditCard.ExpiryDate, Nothing)
                            cmd.AddInParameter("@cc_start_date", SqlDbType.VarChar, 10).Value = Cast.NullIfDefault(oPaymentCashListItem.CreditCard.StartDate, Nothing)
                            cmd.AddInParameter("@cc_issue", SqlDbType.VarChar, 2).Value = Cast.NullIfDefault(oPaymentCashListItem.CreditCard.Issue, Nothing)
                            cmd.AddInParameter("@cc_pin", SqlDbType.VarChar, 20).Value = Cast.NullIfDefault(oPaymentCashListItem.CreditCard.Pin, Nothing)
                            cmd.AddInParameter("@cc_auth_code", SqlDbType.VarChar, 50).Value = Cast.NullIfDefault(oPaymentCashListItem.CreditCard.AuthCode, Nothing)
                            cmd.AddInParameter("@cc_manual_auth_code", SqlDbType.VarChar, 50).Value = Cast.NullIfDefault(oPaymentCashListItem.CreditCard.ManualAuthCode, Nothing)
                            cmd.AddInParameter("@cc_transaction_code", SqlDbType.VarChar, 255).Value = Cast.NullIfDefault(oPaymentCashListItem.CreditCard.TransactionCode, Nothing)
                            cmd.AddInParameter("@cc_customer", SqlDbType.VarChar, 50).Value = IIf(oPaymentCashListItem.CreditCard.CustomerPresent, "Present", "NotPresent").ToString
                            cmd.AddInParameter("@cc_tracking_number", SqlDbType.VarChar, 255).Value = Cast.NullIfDefault(oPaymentCashListItem.CreditCard.TrackingNumber, Nothing)
                            cmd.AddInParameter("@party_bank_id", SqlDbType.Int).Value = Cast.NullIfDefault(oPaymentCashListItem.CreditCard.PartyBankKey, 0)
                        Else
                            cmd.AddInParameter("@cc_name", SqlDbType.VarChar, 50).Value = SqlString.Null
                            cmd.AddInParameter("@cc_number", SqlDbType.VarChar, 30).Value = SqlString.Null
                            cmd.AddInParameter("@cc_expiry_date", SqlDbType.VarChar, 10).Value = SqlString.Null
                            cmd.AddInParameter("@cc_start_date", SqlDbType.VarChar, 10).Value = SqlString.Null
                            cmd.AddInParameter("@cc_issue", SqlDbType.VarChar, 2).Value = SqlString.Null
                            cmd.AddInParameter("@cc_pin", SqlDbType.VarChar, 20).Value = SqlString.Null
                            cmd.AddInParameter("@cc_auth_code", SqlDbType.VarChar, 50).Value = SqlString.Null
                            cmd.AddInParameter("@cc_manual_auth_code", SqlDbType.VarChar, 50).Value = SqlString.Null
                            cmd.AddInParameter("@cc_transaction_code", SqlDbType.VarChar, 255).Value = SqlString.Null
                            cmd.AddInParameter("@cc_customer", SqlDbType.VarChar, 50).Value = SqlString.Null
                            cmd.AddInParameter("@cc_tracking_number", SqlDbType.VarChar, 255).Value = SqlString.Null
                        End If

                        'Bank details
                        If oPaymentCashListItem.Bank IsNot Nothing Then
                            cmd.AddInParameter("@payment_name", SqlDbType.VarChar, 255).Value = GetPaymentName(oPaymentCashListItem.Bank.PayeeName)
                            cmd.AddInParameter("@payment_account_code", SqlDbType.VarChar, 60).Value = Cast.NullIfDefault(oPaymentCashListItem.Bank.AccountCode, Nothing)
                            cmd.AddInParameter("@payment_branch_code", SqlDbType.VarChar, 30).Value = Cast.NullIfDefault(oPaymentCashListItem.Bank.BranchCode, Nothing)
                            If oPaymentCashListItem.Bank.ExpiryDateSpecified Then
                                cmd.AddInParameter("@payment_expiry_date", SqlDbType.DateTime).Value = Cast.NullIfDefault(oPaymentCashListItem.Bank.ExpiryDate, DBNull.Value) 'DateTime.MinValue)
                            Else
                                cmd.AddInParameter("@payment_expiry_date", SqlDbType.DateTime).Value = SqlDateTime.Null
                            End If
                            cmd.AddInParameter("@payment_reference1", SqlDbType.VarChar, 30).Value = Cast.NullIfDefault(oPaymentCashListItem.Bank.Reference1, Nothing)
                            cmd.AddInParameter("@payment_reference2", SqlDbType.VarChar, 30).Value = Cast.NullIfDefault(oPaymentCashListItem.Bank.Reference2, Nothing)
                            cmd.AddInParameter("@party_bank_id", SqlDbType.Int).Value = Cast.NullIfDefault(oPaymentCashListItem.Bank.PartyBankKey, 0)
                            cmd.AddInParameter("@sBusinessIdentifierCode", SqlDbType.VarChar, 50).Value = Cast.NullIfDefault(oPaymentCashListItem.Bank.BIC, Nothing)
                            cmd.AddInParameter("@sInternationalBankAccountNumber", SqlDbType.VarChar, 50).Value = Cast.NullIfDefault(oPaymentCashListItem.Bank.IBAN, Nothing)
                        Else
                            cmd.AddInParameter("@payment_name", SqlDbType.VarChar, 255).Value = SqlString.Null
                            cmd.AddInParameter("@payment_account_code", SqlDbType.VarChar, 60).Value = SqlString.Null
                            cmd.AddInParameter("@payment_branch_code", SqlDbType.VarChar, 30).Value = SqlString.Null
                            cmd.AddInParameter("@payment_expiry_date", SqlDbType.DateTime).Value = SqlDateTime.Null
                            cmd.AddInParameter("@payment_reference1", SqlDbType.VarChar, 30).Value = SqlString.Null
                            cmd.AddInParameter("@payment_reference2", SqlDbType.VarChar, 30).Value = SqlString.Null
                            cmd.AddInParameter("@sBusinessIdentifierCode", SqlDbType.VarChar, 50).Value = SqlString.Null
                            cmd.AddInParameter("@sInternationalBankAccountNumber", SqlDbType.VarChar, 50).Value = SqlString.Null

                        End If

                        con.ExecuteNonQuery(cmd)

                        oCashListItem.CashListItemKey = Cast.ToInt32(cmd.Parameters.Item("@cashlistitem_id").Value, 0)
                    End Using

                ElseIf oCashListItem.GetType Is GetType(BaseReceiptCashListItemType) Then
                    'Currently, we are not processing receipts in this method
                    'Add invalid data error  - payment cash list can not contain reciept cash list item 
                    oSAMErrorCollection.AddInvalidData(SAMInvalidData.ReceiptCashListItemProcessingIsNotSupported,
                                                      "Cash list of type Payment can not contain cash list item of type Receipt",
                                                       "CashListItem")

                Else
                    'Unknown CashListItem type
                    'Add invalid data error  -invalid cashlist item type specified
                    oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidCashListItemType,
                        "Unknown cashlist item type",
                        "CashListItem")
                End If

            ElseIf oCashList.GetType Is GetType(BaseReceiptCashListType) Then
                'Currently, we are not processing receipts in this method
                'Add invalid data error  - receipt cashList processing is not supported 

                'oSAMErrorCollection.AddInvalidData(SAMInvalidData.ReceiptCashListProcessingIsNotSupported, _
                '                                   "Receipt handling is not supported", _
                '                                   "CashList")               
                Dim oReceiptCashList As BaseReceiptCashListType
                oReceiptCashList = DirectCast(oCashList, BaseReceiptCashListType)

                If oCashListItem.GetType Is GetType(BaseReceiptCashListItemType) Then

                    Dim oReceiptCashListItem As BaseReceiptCashListItemType
                    oReceiptCashListItem = DirectCast(oCashListItem, BaseReceiptCashListItemType)

                    Dim ibatchid As Integer
                    Dim ibatchidresult As Integer

                    If Not String.IsNullOrEmpty(oReceiptCashListItem.BankReference) Then
                        Using cmdbatch As SiriusCommand = SiriusCommand.FromProcedure("spu_ACT_Select_Batch_FromBatchRef")
                            cmdbatch.AddOutParameter("@batch_id", SqlDbType.Int)
                            cmdbatch.AddInParameter("@batch_ref", SqlDbType.VarChar, 25).Value = oReceiptCashListItem.BankReference
                            con.ExecuteNonQuery(cmdbatch)
                            ibatchid = Cast.ToInt32(cmdbatch.Parameters.Item("@batch_id").Value, 0)
                        End Using

                        If ibatchid = 0 Then

                            Using cmdaddbatch As SiriusCommand = SiriusCommand.FromProcedure("spu_ACT_Add_Batch")
                                cmdaddbatch.AddOutParameter("@batch_id", SqlDbType.Int)
                                cmdaddbatch.AddInParameter("@company_id", SqlDbType.SmallInt).Value = 1
                                cmdaddbatch.AddInParameter("@batchstatus_id", SqlDbType.SmallInt).Value = 1
                                cmdaddbatch.AddInParameter("@user_id", SqlDbType.SmallInt).Value = 1
                                cmdaddbatch.AddInParameter("@batch_ref", SqlDbType.VarChar, 25).Value = oReceiptCashListItem.BankReference
                                cmdaddbatch.AddInParameter("@created_date", SqlDbType.DateTime).Value = DateTime.Now()
                                cmdaddbatch.AddInParameter("@authorised_date", SqlDbType.DateTime).Value = SqlDateTime.Null
                                cmdaddbatch.AddInParameter("@accounting_date", SqlDbType.DateTime).Value = SqlDateTime.Null
                                cmdaddbatch.AddInParameter("@comment", SqlDbType.VarChar, 60).Value = SqlString.Null
                                cmdaddbatch.AddInParameter("@batch_type_id", SqlDbType.Int).Value = 3
                                cmdaddbatch.AddInParameter("@batch_source_id", SqlDbType.Int).Value = SqlInt32.Null
                                cmdaddbatch.AddInParameter("@xml_object", SqlDbType.VarChar, 1000).Value = SqlString.Null
                                cmdaddbatch.AddInParameter("@exportdate", SqlDbType.DateTime).Value = SqlDateTime.Null
                                cmdaddbatch.AddInParameter("@reexportdate", SqlDbType.DateTime).Value = SqlDateTime.Null
                                cmdaddbatch.AddInParameter("@mediatypeid", SqlDbType.Int).Value = SqlInt32.Null
                                cmdaddbatch.AddInParameter("@totalamount", SqlDbType.Decimal, 9, 4).Value = SqlDecimal.Null
                                cmdaddbatch.AddInParameter("@totaltransactions", SqlDbType.Int).Value = SqlInt32.Null
                                cmdaddbatch.AddInParameter("@importeddate", SqlDbType.DateTime).Value = SqlDateTime.Null
                                cmdaddbatch.AddInParameter("@rejectamount", SqlDbType.Decimal, 9, 4).Value = SqlDecimal.Null
                                cmdaddbatch.AddInParameter("@rejecttransactions", SqlDbType.Int).Value = SqlInt32.Null
                                cmdaddbatch.AddInParameter("@closeddate", SqlDbType.DateTime).Value = SqlDateTime.Null
                                cmdaddbatch.AddInParameter("@interfacecode", SqlDbType.VarChar, 30).Value = SqlString.Null
                                cmdaddbatch.AddInParameter("@autoclose", SqlDbType.TinyInt).Value = 0

                                con.ExecuteNonQuery(cmdaddbatch)
                                ibatchidresult = Cast.ToInt32(cmdaddbatch.Parameters.Item("@batch_id").Value, 0)
                            End Using
                        Else
                            ibatchidresult = ibatchid
                        End If
                    End If

                    Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Add_CashListItem_Receipt")

                        If String.IsNullOrEmpty(oReceiptCashListItem.TaxBandCode) = False Then
                            oReceiptCashListItem.TaxBandKey = GetAndValidateSpecifiedTableCode(con,
                                        PMLookupTable.TaxBand,
                                        "Tax_Band_Id",
                                        "code",
                                        oReceiptCashListItem.TaxBandCode,
                                        oSAMErrorCollection, "TaxBandCode")
                        End If

                        'Order of parameters has been rearranged 
                        cmd.AddOutParameter("@cashlistitem_id", SqlDbType.Int)

                        If (oReceiptCashListItem.IsProduceDocument = True) Then
                            cmd.AddInParameter("@letter", SqlDbType.TinyInt).Value = 1
                        Else
                            cmd.AddInParameter("@letter", SqlDbType.TinyInt).Value = 0
                        End If
                        cmd.AddInParameter("@batch_id", SqlDbType.Int).Value = Cast.NullIfDefault(ibatchidresult, 0)

                        cmd.AddInParameter("@cashlist_id", SqlDbType.Int).Value = Cast.NullIfDefault(oReceiptCashList.CashListKey, 0)
                        cmd.AddInParameter("@account_code", SqlDbType.VarChar, 30).Value = Cast.NullIfDefault(oReceiptCashListItem.AccountShortCode, Nothing)
                        cmd.AddInParameter("@transaction_date", SqlDbType.DateTime).Value = Cast.NullIfDefault(oReceiptCashListItem.TransactionDate, DateTime.MinValue)
                        cmd.AddInParameter("@amount", SqlDbType.Money).Value = Cast.NullIfDefault(CType((oReceiptCashListItem.Amount), Double), 0.0)
                        cmd.AddInParameter("@Amount_tendered", SqlDbType.Money).Value = Cast.NullIfDefault((oReceiptCashListItem.Amount_Tendered), 0.0)
                        cmd.AddInParameter("@Original_amount", SqlDbType.Money).Value = Cast.NullIfDefault((oReceiptCashListItem.Original_Amount), 0.0)

                        If Not String.IsNullOrEmpty(oReceiptCashListItem.MediaReference) Then
                            If oReceiptCashListItem.MediaReference.Contains("INSDEPOSIT") Then
                                Dim sMediaRef As String = oReceiptCashListItem.MediaReference.Replace("INSDEPOSIT", "")
                                cmd.AddInParameter("@media_ref", SqlDbType.VarChar, 100).Value = Cast.NullIfDefault(sMediaRef, Nothing)
                            Else
                                cmd.AddInParameter("@media_ref", SqlDbType.VarChar, 100).Value = Cast.NullIfDefault(oReceiptCashListItem.MediaReference, Nothing)
                            End If
                        Else
                            cmd.AddInParameter("@media_ref", SqlDbType.VarChar, 100).Value = Cast.NullIfDefault(oReceiptCashListItem.MediaReference, Nothing)
                        End If
                        cmd.AddInParameter("@our_ref", SqlDbType.VarChar, 255).Value = Cast.NullIfDefault(oReceiptCashListItem.OurReference, Nothing)
                        cmd.AddInParameter("@their_ref", SqlDbType.VarChar, 30).Value = Cast.NullIfDefault(oReceiptCashListItem.TheirReference, Nothing)
                        cmd.AddInParameter("@contact_name", SqlDbType.VarChar, 255).Value = Cast.NullIfDefault(oReceiptCashListItem.ContactName, Nothing)

                        cmd.AddInParameter("@receipt_type_id", SqlDbType.Int).Value = Cast.NullIfDefault(oReceiptCashListItem.TypeKey, 0)
                        cmd.AddInParameter("@receipt_status_id", SqlDbType.Int).Value = Cast.NullIfDefault(oReceiptCashListItem.StatusKey, 0)
                        cmd.AddInParameter("@allocationstatus_id", SqlDbType.Int).Value = Cast.NullIfDefault(oReceiptCashListItem.AllocationStatusKey, 0)
                        cmd.AddInParameter("@mediatype_id", SqlDbType.Int).Value = Cast.NullIfDefault(oReceiptCashListItem.MediaTypeKey, 0)
                        cmd.AddInParameter("@pmuser_id", SqlDbType.Int).Value = _SiriusUser.UserID
                        cmd.AddInParameter("@receipt_details", SqlDbType.VarChar, 500).Value = Cast.NullIfDefault(oReceiptCashListItem.FurtherDetails, Nothing)

                        cmd.AddInParameter("@ntax_band_id", SqlDbType.Int).Value = Cast.NullIfDefault(oReceiptCashListItem.TaxBandKey, 0)

                        'Contact address
                        If oReceiptCashListItem.ContactAddress IsNot Nothing Then
                            cmd.AddInParameter("@address1", SqlDbType.VarChar, 60).Value = Cast.NullIfDefault(oReceiptCashListItem.ContactAddress.AddressLine1, Nothing)
                            cmd.AddInParameter("@address2", SqlDbType.VarChar, 60).Value = Cast.NullIfDefault(oReceiptCashListItem.ContactAddress.AddressLine2, Nothing)
                            cmd.AddInParameter("@address3", SqlDbType.VarChar, 60).Value = Cast.NullIfDefault(oReceiptCashListItem.ContactAddress.AddressLine3, Nothing)
                            cmd.AddInParameter("@address4", SqlDbType.VarChar, 60).Value = Cast.NullIfDefault(oReceiptCashListItem.ContactAddress.AddressLine4, Nothing)
                            cmd.AddInParameter("@postal_code", SqlDbType.VarChar, 20).Value = Cast.NullIfDefault(oReceiptCashListItem.ContactAddress.PostCode, Nothing)
                            cmd.AddInParameter("@address_country_id", SqlDbType.Int).Value = Cast.NullIfDefault(oReceiptCashListItem.ContactAddress.CountryKey, 0)
                        Else
                            cmd.AddInParameter("@address1", SqlDbType.VarChar, 60).Value = SqlString.Null
                            cmd.AddInParameter("@address2", SqlDbType.VarChar, 60).Value = SqlString.Null
                            cmd.AddInParameter("@address3", SqlDbType.VarChar, 60).Value = SqlString.Null
                            cmd.AddInParameter("@address4", SqlDbType.VarChar, 60).Value = SqlString.Null
                            cmd.AddInParameter("@postal_code", SqlDbType.VarChar, 20).Value = SqlString.Null
                            cmd.AddInParameter("@address_country_id", SqlDbType.Int).Value = SqlInt16.Null
                        End If

                        'Credit card details
                        If oReceiptCashListItem.CreditCard IsNot Nothing Then
                            cmd.AddInParameter("@cc_name", SqlDbType.VarChar, 50).Value = Cast.NullIfDefault(oReceiptCashListItem.CreditCard.NameOnCreditCard, Nothing)
                            cmd.AddInParameter("@cc_number", SqlDbType.VarChar, 30).Value = Cast.NullIfDefault(oReceiptCashListItem.CreditCard.Number, Nothing)
                            cmd.AddInParameter("@cc_expiry_date", SqlDbType.VarChar, 10).Value = Cast.NullIfDefault(oReceiptCashListItem.CreditCard.ExpiryDate, Nothing)
                            cmd.AddInParameter("@cc_start_date", SqlDbType.VarChar, 10).Value = Cast.NullIfDefault(oReceiptCashListItem.CreditCard.StartDate, Nothing)
                            cmd.AddInParameter("@cc_issue", SqlDbType.VarChar, 2).Value = Cast.NullIfDefault(oReceiptCashListItem.CreditCard.Issue, Nothing)
                            cmd.AddInParameter("@cc_pin", SqlDbType.VarChar, 20).Value = Cast.NullIfDefault(oReceiptCashListItem.CreditCard.Pin, Nothing)
                            cmd.AddInParameter("@cc_auth_code", SqlDbType.VarChar, 50).Value = Cast.NullIfDefault(oReceiptCashListItem.CreditCard.AuthCode, Nothing)
                            cmd.AddInParameter("@cc_manual_auth_code", SqlDbType.VarChar, 50).Value = Cast.NullIfDefault(oReceiptCashListItem.CreditCard.ManualAuthCode, Nothing)
                            cmd.AddInParameter("@cc_transaction_code", SqlDbType.VarChar, 255).Value = Cast.NullIfDefault(oReceiptCashListItem.CreditCard.TransactionCode, Nothing)
                            cmd.AddInParameter("@cc_customer", SqlDbType.VarChar, 50).Value = IIf(oReceiptCashListItem.CreditCard.CustomerPresent, "Present", "NotPresent").ToString

                            cmd.AddInParameter("@cc_tracking_number", SqlDbType.VarChar, 255).Value = Cast.NullIfDefault(oReceiptCashListItem.CreditCard.TrackingNumber, Nothing)
                            cmd.AddInParameter("@party_bank_id", SqlDbType.Int).Value = Cast.NullIfDefault(oReceiptCashListItem.CreditCard.PartyBankKey, 0)
                        Else
                            cmd.AddInParameter("@cc_name", SqlDbType.VarChar, 50).Value = SqlString.Null
                            cmd.AddInParameter("@cc_number", SqlDbType.VarChar, 30).Value = SqlString.Null
                            cmd.AddInParameter("@cc_expiry_date", SqlDbType.VarChar, 10).Value = SqlString.Null
                            cmd.AddInParameter("@cc_start_date", SqlDbType.VarChar, 10).Value = SqlString.Null
                            cmd.AddInParameter("@cc_issue", SqlDbType.VarChar, 2).Value = SqlString.Null
                            cmd.AddInParameter("@cc_pin", SqlDbType.VarChar, 20).Value = SqlString.Null
                            cmd.AddInParameter("@cc_auth_code", SqlDbType.VarChar, 50).Value = SqlString.Null
                            cmd.AddInParameter("@cc_manual_auth_code", SqlDbType.VarChar, 50).Value = SqlString.Null
                            cmd.AddInParameter("@cc_transaction_code", SqlDbType.VarChar, 255).Value = SqlString.Null
                            cmd.AddInParameter("@cc_customer", SqlDbType.VarChar, 50).Value = SqlString.Null

                            cmd.AddInParameter("@cc_tracking_number", SqlDbType.VarChar, 255).Value = SqlString.Null

                        End If

                        'Bank details
                        If oReceiptCashListItem.Bank IsNot Nothing Then
                            cmd.AddInParameter("@cheque_date", SqlDbType.DateTime).Value = Cast.DefaultIfNull(oReceiptCashListItem.Bank.ChequeDate, Nothing)
                            cmd.AddInParameter("@bank_code", SqlDbType.VarChar, 30).Value = Cast.NullIfDefault(oReceiptCashListItem.Bank.BankCode, Nothing)
                            cmd.AddInParameter("@payment_name", SqlDbType.VarChar, 255).Value = GetPaymentName(oReceiptCashListItem.Bank.PayerName)

                            cmd.AddInParameter("@party_bank_id", SqlDbType.Int).Value = Cast.NullIfDefault(oReceiptCashListItem.Bank.PartyBankKey, 0)
                        Else
                            cmd.AddInParameter("@cheque_date", SqlDbType.VarChar, 60).Value = SqlDateTime.Null
                            cmd.AddInParameter("@bank_code", SqlDbType.VarChar, 60).Value = SqlString.Null
                            cmd.AddInParameter("@payment_name", SqlDbType.VarChar, 255).Value = SqlString.Null
                        End If

                        con.ExecuteNonQuery(cmd)

                        oCashListItem.CashListItemKey = Cast.ToInt32(cmd.Parameters.Item("@cashlistitem_id").Value, 0)

                        If oReceiptCashListItem.TypeCode = "INST" Then
                            If oReceiptCashListItem.InstalmentPlanDetails IsNot Nothing Then
                                For iCount As Integer = 0 To oReceiptCashListItem.InstalmentPlanDetails.GetUpperBound(0)
                                    If oReceiptCashListItem.InstalmentPlanDetails(iCount).InstalmentDetails IsNot Nothing Then
                                        For iCount1 As Integer = 0 To oReceiptCashListItem.InstalmentPlanDetails(iCount).InstalmentDetails.GetUpperBound(0)
                                            If oReceiptCashListItem.InstalmentPlanDetails(iCount).InstalmentDetails(iCount1).iPFInstalmentID <> 0 Then
                                                Using cmdInstallement As SiriusCommand = SiriusCommand.FromProcedure("spu_ACT_Add_CashListItem_Instalment")
                                                    cmdInstallement.AddInParameter("@cashlistitem_id", SqlDbType.Int).Value = oCashListItem.CashListItemKey
                                                    cmdInstallement.AddInParameter("@PFInstalments_id", SqlDbType.Int).Value = oReceiptCashListItem.InstalmentPlanDetails(iCount).InstalmentDetails(iCount1).iPFInstalmentID
                                                    con.ExecuteNonQuery(cmdInstallement)
                                                End Using
                                            End If
                                        Next
                                    End If
                                Next

                                PostInstalments(con, oReceiptCashListItem.InstalmentPlanDetails, oCashListItem.CashListItemKey)

                            End If
                        End If
                    End Using
                End If
            Else
                'Unknown CashList type
                'Add invalid data error  -invalid cashlist type specified
                oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidCashListType,
                                                   "Unknown cashlist type",
                                                   "CashList")
            End If
        Else 'Either CashList or CashListItem is empty
            If (oCashList Is Nothing) Then 'Cashlist is empty
                oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                                   SAMInvalidData.MandatoryInputMissing.ToString,
                                                   "CashList")
            Else 'Cashlist item is empty
                oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                                   SAMInvalidData.MandatoryInputMissing.ToString,
                                                   "CashListItem")
            End If
        End If
    End Sub

    Private Sub PostInstalments(ByVal con As SiriusConnection, ByVal instalmentPlanDetails() As BaseCoreCashListItemTypeInstalmentPlanDetails, ByVal cashListItemId As Integer)

        Dim pfInstalments As New bSIRPFInstalments.Business
        Dim count As Integer
        Dim vInstalmentIDArray(,) As Object
        Dim lIDArrayCount As Long
        Dim oSAMErrorCollection As New SAMErrorCollection

        SAMFunc.InitialiseSBOObject(con, pfInstalments, _SiriusUser, sObjectName:="bSIRPFInstalments.Business")

        lIDArrayCount = 0

        If instalmentPlanDetails IsNot Nothing Then

            'increase size of array by one (unless first time through)
            ReDim Preserve vInstalmentIDArray(5, instalmentPlanDetails.GetUpperBound(0))
            Dim bIsPartialPayment As Boolean = False
            Dim bIsWriteOffPayment As Boolean = False
            Dim dActualAmount As Double = 0
            'First loop through the instalments to apply the changes to complete collection
            For bInstalmentCount As Integer = 0 To instalmentPlanDetails.GetUpperBound(0)
                If instalmentPlanDetails(bInstalmentCount).InstalmentDetails IsNot Nothing Then
                    For nRowCount As Integer = 0 To instalmentPlanDetails(bInstalmentCount).InstalmentDetails.GetUpperBound(0)
                        If instalmentPlanDetails(bInstalmentCount).InstalmentDetails(nRowCount).iPFInstalmentID <> 0 Then
                            If instalmentPlanDetails(bInstalmentCount).InstalmentDetails(nRowCount).IsPartialPayment Then
                                bIsPartialPayment = True
                                Exit For
                            ElseIf instalmentPlanDetails(bInstalmentCount).InstalmentDetails(nRowCount).IsWriteOffPayment Then
                                dActualAmount = instalmentPlanDetails(0).InstalmentDetails(nRowCount).Amount
                                bIsWriteOffPayment = True
                                Exit For
                            End If
                        End If
                    Next
                End If
            Next

            For iCount As Integer = 0 To instalmentPlanDetails.GetUpperBound(0)
                If instalmentPlanDetails(iCount).InstalmentDetails IsNot Nothing Then
                    For iCount1 As Integer = 0 To instalmentPlanDetails(iCount).InstalmentDetails.GetUpperBound(0)
                        If instalmentPlanDetails(iCount).InstalmentDetails(iCount1).iPFInstalmentID <> 0 Then
                            vInstalmentIDArray(0, count) = instalmentPlanDetails(iCount).InstalmentDetails(iCount1).iPFInstalmentID
                            If bIsWriteOffPayment AndAlso (Not instalmentPlanDetails(iCount).InstalmentDetails(iCount1).IsWriteOffPayment) Then
                                vInstalmentIDArray(2, count) = 0
                                vInstalmentIDArray(1, count) = dActualAmount

                            ElseIf (instalmentPlanDetails(iCount).InstalmentDetails(iCount1).IsWriteOffPayment) Then
                                vInstalmentIDArray(2, count) = 0
                            Else
                                vInstalmentIDArray(2, count) = 2
                            End If

                            If instalmentPlanDetails(iCount).InstalmentDetails(iCount1).IsPartialPayment OrElse instalmentPlanDetails(iCount).InstalmentDetails(iCount1).IsWriteOffPayment Then
                                If (instalmentPlanDetails(iCount).InstalmentDetails(iCount1).OverPaymentWriteOffAmount <> 0.0) Then
                                    vInstalmentIDArray(1, count) = instalmentPlanDetails(iCount).InstalmentDetails(iCount1).OverPaymentWriteOffAmount
                                Else
                                    vInstalmentIDArray(1, count) = instalmentPlanDetails(iCount).InstalmentDetails(iCount1).Amount
                                End If
                            ElseIf (Not bIsWriteOffPayment) Then
                                vInstalmentIDArray(1, count) = 0
                            End If
                            If instalmentPlanDetails(iCount).InstalmentDetails(iCount1).IsWriteOffPayment Then
                                If instalmentPlanDetails.Length = 1 Then
                                    If instalmentPlanDetails(iCount).InstalmentDetails(iCount1).ActualAmount <> 0.0 Then
                                        vInstalmentIDArray(3, count) = instalmentPlanDetails(iCount).InstalmentDetails(iCount1).ActualAmount
                                    End If
                                Else
                                    vInstalmentIDArray(3, count) = dActualAmount
                                End If

                                vInstalmentIDArray(5, count) = instalmentPlanDetails(iCount).InstalmentDetails(iCount1).WriteOffReasonID
                            Else
                                vInstalmentIDArray(3, count) = instalmentPlanDetails(iCount).InstalmentDetails(iCount1).Amount
                                vInstalmentIDArray(5, count) = 0
                            End If

                            vInstalmentIDArray(4, count) = 1


                            count += 1
                        End If
                    Next
                End If
            Next

            'the instalment is part of the receipt
            Dim ret As Integer = pfInstalments.PostMultipleInstalments(v_vInstalmentID:=vInstalmentIDArray,
                                                             v_lCashDrawerID:=0,
                                                             v_dtTransactionDate:=Now,
                                                             v_vCashListItemID:=cashListItemId)

            If ret <> PMEReturnCode.PMTrue Then
                ' if the account processing fails then throw a business rule error
                oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed,
                                                    SAMBusinessErrors.COMComponentMethodFailed.ToString,
                                                    "bSIRPFInstalments.Business.PostMultipleInstalments Failed for CashListItem Id - " & cashListItemId.ToString)
                oSAMErrorCollection.CheckForErrors()
            End If

        End If
        pfInstalments.Dispose()
        pfInstalments = Nothing

    End Sub

    ''' <summary>
    ''' Posts a cashlist item for agent receipt
    ''' </summary>
    '''<param name="con">An object of SiriusConnection class</param>
    '''<param name="oCashListItem">An object of BaseCoreCashListItemType class</param>
    '''<param name="iCashListID">Cash list key</param>
    '''<param name="oSAMErrorCollection">An object of SAMErrorCollection class</param>
    Public Sub PostCashListItem(ByVal con As SiriusConnection,
                                ByRef oCashListItem As BaseCoreCashListItemType,
                                ByVal iCashListID As Integer,
                                ByRef oSAMErrorCollection As SAMErrorCollection)

        If oCashListItem IsNot Nothing Then
            Dim oBusiness As New CoreBusiness
            Dim sChequeProductionEnabledOption As String = ""
            Dim bChequeProductionEnabledOption As Boolean = False
            oBusiness.GetSystemOption(oCashListItem.BranchCode, SystemOption.ChequeProductionEnabled,
                                      sChequeProductionEnabledOption)

            If Not (Trim(sChequeProductionEnabledOption) = "0") Then
                bChequeProductionEnabledOption = True
            End If

            If oCashListItem.GetType Is GetType(BasePaymentCashListItemType) Then

                Dim oPaymentCashListItem As BasePaymentCashListItemType
                oPaymentCashListItem = DirectCast(oCashListItem, BasePaymentCashListItemType)

                Dim oCashListPost As bACTCashListPost.Automated = Nothing

                Dim lReturn As Integer

                Try

                    ' instantiate the business object
                    oCashListPost = New bACTCashListPost.Automated
                    'Following line is being added by rk as part of SAM Interop conversions and as per internal and SSP discussion in place of LOC next to it (now commented somewhere below saying oDatabase = con.PMDAODatabase)

                    Dim oDatabaseObject As Object = Nothing
                    If Not con Is Nothing Then
                        oDatabaseObject = Nothing
                        oDatabaseObject = con.PMDAODatabase
                    End If


                    ' initialise the business object
                    lReturn = CInt(oCashListPost.Initialise(_SiriusUser.Username,
                                                       _SiriusUser.Password,
                                                       _SiriusUser.UserID,
                                                       _SiriusUser.SourceID,
                                                       _SiriusUser.LanguageID,
                                                       _SiriusUser.CurrencyID,
                                                       1,
                                                       SiriusUserDefaults.AppName,
                                                     False, oDatabaseObject))

                    If (lReturn <> PMEReturnCode.PMTrue) Then

                        ' if the account processing fails then throw a business rule error
                        oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed,
                                                            SAMBusinessErrors.COMComponentMethodFailed.ToString,
                                                            "bACTCashListPost.Automated.Initialise")
                        oSAMErrorCollection.CheckForErrors()

                    End If

                    ' set the process modes
                    lReturn = oCashListPost.SetProcessModes(0, 0, 0, 0, Date.Today)
                    If (lReturn <> PMEReturnCode.PMTrue) Then
                        ' if the account processing fails then throw a business rule error
                        oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed,
                                                            SAMBusinessErrors.COMComponentMethodFailed.ToString,
                                                            "bACTCashListPost.Automated.SetProcessModes")
                        oSAMErrorCollection.CheckForErrors()
                    End If
                    oCashListPost.ChequeProduction = bChequeProductionEnabledOption

                    ' Post the unallocated cash
                    lReturn = oCashListPost.PostUnallocatedCash(
                        v_vCashListID:=iCashListID,
                        v_vCashListItemID:=oCashListItem.CashListItemKey,
                        v_dTransactionDate:=oCashListItem.TransactionDate)

                    If (lReturn <> PMEReturnCode.PMTrue) Then
                        ' if the account processing fails then throw a business rule error
                        oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.AccountsProcessingFailed,
                                                            SAMBusinessErrors.AccountsProcessingFailed.ToString,
                                                            "bACTCashListPost.Automated.PostUnallocatedCash")
                        oSAMErrorCollection.CheckForErrors()
                    End If

                    oCashListItem.TransDetailKey = oCashListPost.CashTransId

                Finally
                    If oCashListPost IsNot Nothing Then
                        oCashListPost.Dispose()
                        oCashListPost = Nothing
                    End If
                End Try
            ElseIf oCashListItem.GetType Is GetType(BaseReceiptCashListItemType) Then
                'Currently, we are not processing receipts in this method
                'Add invalid data error  - receipt cashList item processing is not supported 

                'oSAMErrorCollection.AddInvalidData(SAMInvalidData.ReceiptCashListItemProcessingIsNotSupported, _
                '                                   "Receipt item handling is not supported", _
                '                                   "CashListItem")

                Dim oReceiptCashListItem As BaseReceiptCashListItemType
                oReceiptCashListItem = DirectCast(oCashListItem, BaseReceiptCashListItemType)

                Dim oCashListPost As bACTCashListPost.Automated = Nothing

                Dim lReturn As Integer

                Try

                    ' instantiate the business object
                    oCashListPost = New bACTCashListPost.Automated

                    'Following line is being added by rk as part of SAM Interop conversions and as per internal and SSP discussion in place of LOC next to it (now commented somewhere below saying oDatabase = con.PMDAODatabase)

                    Dim oDatabaseObject As Object = Nothing
                    Dim dBaseAmount As Decimal
                    If Not con Is Nothing Then
                        oDatabaseObject = Nothing
                        oDatabaseObject = con.PMDAODatabase
                    End If

                    'Dim oDatabaseObject As Object = con.PMDAODatabase

                    ' initialise the business object
                    lReturn = CInt(oCashListPost.Initialise(_SiriusUser.Username,
                                                       _SiriusUser.Password,
                                                       _SiriusUser.UserID,
                                                       _SiriusUser.SourceID,
                                                       _SiriusUser.LanguageID,
                                                       _SiriusUser.CurrencyID,
                                                       1,
                                                       SiriusUserDefaults.AppName,
                                                       False, oDatabaseObject))

                    If (lReturn <> PMEReturnCode.PMTrue) Then

                        ' if the account processing fails then throw a business rule error
                        oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed,
                                                            SAMBusinessErrors.COMComponentMethodFailed.ToString,
                                                            "bACTCashListPost.Automated.Initialise")
                        oSAMErrorCollection.CheckForErrors()

                    End If

                    ' set the process modes
                    lReturn = oCashListPost.SetProcessModes(0, 0, 0, 0, Date.Today)
                    If (lReturn <> PMEReturnCode.PMTrue) Then
                        ' if the account processing fails then throw a business rule error
                        oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed,
                                                            SAMBusinessErrors.COMComponentMethodFailed.ToString,
                                                            "bACTCashListPost.Automated.SetProcessModes")
                        oSAMErrorCollection.CheckForErrors()
                    End If

                    If Not String.IsNullOrEmpty(oCashListItem.MediaReference) Then
                        If oCashListItem.MediaReference.Contains("INSDEPOSIT") Then
                            Dim sMediaRef As String = oCashListItem.MediaReference.Replace("INSDEPOSIT", "")
                            ' Post the unallocated cash
                            lReturn = oCashListPost.PostUnallocatedCash(
                                v_vCashListID:=iCashListID,
                                sFailureReason:=String.Empty,
                                sInsurence_Ref:=sMediaRef)
                        Else
                            ' Post the unallocated cash
                            lReturn = oCashListPost.PostUnallocatedCash(
                                v_vCashListID:=iCashListID,
                                 r_cBaseAmount:=dBaseAmount,
                                v_vCashListItemID:=oCashListItem.CashListItemKey)
                        End If
                    Else
                        ' Post the unallocated cash
                        lReturn = oCashListPost.PostUnallocatedCash(
                                        v_vCashListID:=iCashListID,
                                        r_cBaseAmount:=dBaseAmount,
                                        v_vCashListItemID:=oCashListItem.CashListItemKey,
                                        v_dTransactionDate:=oCashListItem.TransactionDate)
                    End If

                    If (lReturn <> PMEReturnCode.PMTrue) Then
                        ' if the account processing fails then throw a business rule error
                        oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.AccountsProcessingFailed,
                                                            SAMBusinessErrors.AccountsProcessingFailed.ToString,
                                                            "bACTCashListPost.Automated.PostUnallocatedCash")
                        oSAMErrorCollection.CheckForErrors()
                    End If

                    oCashListItem.TransDetailKey = oCashListPost.CashTransId
                    oCashListItem.Amount = dBaseAmount
                Finally
                    If oCashListPost IsNot Nothing Then
                        oCashListPost.Dispose()
                        oCashListPost = Nothing
                    End If
                End Try

            Else
                'Unknown CashListItem type
                'Add invalid data error  -invalid cashlist item type specified
                oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidCashListItemType,
                                                   "Unknown cashlist item type",
                                                   "CashListItem")
            End If

        Else
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                               SAMInvalidData.MandatoryInputMissing.ToString,
                                               "CashListItem")
        End If
    End Sub

    ''' <summary>
    '''This method pass the request object oGetPaymentCashListItemsRequest along with the connection object
    '''</summary>
    '''<param name="oGetPaymentCashListItemsRequest" type="BaseGetPaymentCashListItemsRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.BaseImplementationTypes.BaseGetPaymentCashListItemsResponseType</returns>
    '''<remarks></remarks>
    Public Overloads Function GetPaymentCashListItems(ByVal oGetPaymentCashListItemsRequest As BaseGetPaymentCashListItemsRequestType) As BaseGetPaymentCashListItemsResponseType

        ' validate the request structure against the specified business rules
        Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)

            Dim oResponse As BaseGetPaymentCashListItemsResponseType

            oResponse = GetPaymentCashListItems(con, oGetPaymentCashListItemsRequest)

            Return oResponse

        End Using

    End Function

    ''' <summary>
    '''This method pass the request object to the stored procedure and the cover note is being added
    '''</summary>
    '''<param name="oGetPaymentCashListItemsRequest" type="BaseGetPaymentCashListItemsRequestType"></param>
    '''<param name="con" type="SiriusConnection"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.BaseImplementationTypes.BaseGetPaymentCashListItemsResponseType</returns>
    '''<remarks></remarks>

    Public Overloads Function GetPaymentCashListItems(ByVal con As SiriusConnection, ByVal oGetPaymentCashListItemsRequest As BaseGetPaymentCashListItemsRequestType) As BaseGetPaymentCashListItemsResponseType

        Dim oGetPaymentCashListItemsResponse As New BaseGetPaymentCashListItemsResponseType
        Dim oCoreBusiness As New CoreBusiness
        Dim oErrors As New SAMErrorCollection
        Dim dsCashListItem As DataSet = Nothing
        Dim dsResultantDataSet As New DataSet
        Dim icount As Int32

        Dim ibranchId As Integer
        Dim nTypeOfPackage As enumTypeOfPackage

        If oGetPaymentCashListItemsRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.GetPaymentCashListItemsRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oGetPaymentCashListItemsResponse = New SAMForInsuranceV2ImplementationTypes.GetPaymentCashListItemsResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If

        ' exit if there are any missing parameters
        oGetPaymentCashListItemsRequest.Validate(CObj(oErrors))
        oErrors.CheckForErrors()

        ' Lookup Codes to ensure they are valid – To be done for all codes in the request, e.g.
        ibranchId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
          PMLookupTable.Source, oGetPaymentCashListItemsRequest.BranchCode, "Source", oErrors)
        oErrors.CheckForErrors()
        GetAndValidateSpecifiedTableCode(con, PMLookupTable.CashList, "cashlist_id", "cashlist_id", oGetPaymentCashListItemsRequest.CashListKey.ToString(), oErrors, "CashListKey")
        oErrors.CheckForErrors()
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_SelAll_CashListItem")
            cmd.AddInParameter("@cashlist_id", SqlDbType.Int).Value = oGetPaymentCashListItemsRequest.CashListKey
            dsCashListItem = con.ExecuteDataSet(cmd, "Row")
        End Using
        If dsCashListItem IsNot Nothing AndAlso dsCashListItem.Tables.Count > 0 AndAlso dsCashListItem.Tables(0).Rows.Count > 0 Then
            With dsCashListItem.Tables(0)
                dsResultantDataSet.Tables.Add()
                dsResultantDataSet.Tables(0).Columns.Add("CashListItemKey", GetType(System.Int32))
                dsResultantDataSet.Tables(0).Columns.Add("MediaReference", GetType(System.String))
                dsResultantDataSet.Tables(0).Columns.Add("MediaType", GetType(System.String))
                dsResultantDataSet.Tables(0).Columns.Add("Amount", GetType(System.Double))
                dsResultantDataSet.Tables(0).Columns.Add("AccountShortCode", GetType(System.String))
                dsResultantDataSet.Tables(0).Columns.Add("Status", GetType(System.String))
                dsResultantDataSet.Tables(0).Columns.Add("Letter", GetType(System.Boolean))
                dsResultantDataSet.Tables(0).Columns.Add("TaxBandCode", GetType(System.String))

                For icount = 0 To .Rows.Count - 1
                    dsResultantDataSet.Tables(0).Rows.Add()
                    dsResultantDataSet.Tables(0).Rows(icount)("CashListItemKey") = .Rows(icount).Item("CashListItemKey")
                    dsResultantDataSet.Tables(0).Rows(icount)("MediaReference") = .Rows(icount).Item("MediaReference")
                    dsResultantDataSet.Tables(0).Rows(icount)("MediaType") = .Rows(icount).Item("MediaType")
                    dsResultantDataSet.Tables(0).Rows(icount)("Amount") = .Rows(icount).Item("Amount")
                    dsResultantDataSet.Tables(0).Rows(icount)("AccountShortCode") = .Rows(icount).Item("AccountShortCode")
                    dsResultantDataSet.Tables(0).Rows(icount)("Status") = .Rows(icount).Item("Status")
                    dsResultantDataSet.Tables(0).Rows(icount)("Letter") = Cast.ToBoolean(.Rows(icount).Item("Letter"))
                    dsResultantDataSet.Tables(0).Rows(icount)("TaxBandCode") = Cast.ToString(.Rows(icount).Item("TaxBandCode"), String.Empty)
                Next

            End With
        Else
            oGetPaymentCashListItemsResponse.ResultData = Nothing
            Return oGetPaymentCashListItemsResponse
        End If
        If oGetPaymentCashListItemsRequest.WCFSecurityToken = "" Then
            Dim Docxml As New System.Xml.XmlDocument
            dsResultantDataSet.DataSetName = "BaseGetPaymentCashListItemsResponseTypePaymentCashListItems"
            dsResultantDataSet.Tables(0).TableName = "Row"
            Docxml.LoadXml(dsResultantDataSet.GetXml)

            oGetPaymentCashListItemsResponse.ResultDataset = Docxml.DocumentElement()
        End If
        oGetPaymentCashListItemsResponse.ResultData = dsResultantDataSet
        Return oGetPaymentCashListItemsResponse

    End Function

    ''' <summary>
    '''This method pass the request object oGetReceiptCashListDetailsRequest along with the connection object
    '''</summary>
    '''<param name="oGetReceiptCashListDetailsRequest" type="BaseGetReceiptCashListDetailsRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.BaseImplementationTypes.BaseGetReceiptCashListDetailsResponseType</returns>
    '''<remarks></remarks>
    Public Overloads Function GetReceiptCashListDetails(ByVal oGetReceiptCashListDetailsRequest As BaseGetReceiptCashListDetailsRequestType) As BaseGetReceiptCashListDetailsResponseType

        ' validate the request structure against the specified business rules
        Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)

            Dim oResponse As BaseGetReceiptCashListDetailsResponseType

            oResponse = GetReceiptCashListDetails(con, oGetReceiptCashListDetailsRequest)

            Return oResponse

        End Using

    End Function

    ''' <summary>
    '''This method pass the request object oGetReceiptCashListDetailsRequest along with the connection object
    '''</summary>
    '''<param name="con" type="SiriusConnection"></param>
    '''<param name="icashListKey" type="Integer"></param>
    ''' <returns>An object of type BaseCoreCashListType</returns>
    '''<remarks></remarks>

    Public Function GetCashListforeceipt(ByVal con As SiriusConnection, ByVal icashListKey As Integer, ByRef oErrors As Object) As BaseCoreCashListType
        Dim dsCashList As New DataSet
        Dim oBusiness As New CoreBusiness
        Dim oBaseReceiptCashListType As New BaseReceiptCashListType

        Dim oErrorCollection As SAMErrorCollection = CType(oErrors, SAMErrorCollection)
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_ACT_Select_CashList")
            cmd.AddInParameter("@cashlist_id", SqlDbType.Int).Value = Cast.NullIfDefault(icashListKey, Nothing)
            dsCashList = con.ExecuteDataSet(cmd, "CashList")
        End Using

        If dsCashList IsNot Nothing AndAlso dsCashList.Tables.Count > 0 AndAlso dsCashList.Tables(0).Rows.Count > 0 Then

            With oBaseReceiptCashListType

                .TypeCode = GetAndValidateDescriptionById(con, "CashListType", "code", "cashlisttype_id", Convert.ToInt32(dsCashList.Tables(0).Rows(0).Item("cashlisttype_id")).ToString())
                .BankAccountCode = GetAndValidateDescriptionById(con, "BankAccount", "code", "bankaccount_id", Convert.ToInt32(dsCashList.Tables(0).Rows(0).Item("bankaccount_id")).ToString())
                .StatusCode = GetAndValidateDescriptionById(con, "CashListStatus", "code", "cashliststatus_id", Convert.ToInt32(dsCashList.Tables(0).Rows(0).Item("cashliststatus_id")).ToString())
                .CurrencyCode = GetAndValidateDescriptionById(con, "Currency", "code", "currency_id", Convert.ToInt32(dsCashList.Tables(0).Rows(0).Item("currency_id")).ToString())
                .ListDate = Convert.ToDateTime(dsCashList.Tables(0).Rows(0).Item("list_date"))
                .Reference = dsCashList.Tables(0).Rows(0).Item("cashlist_ref").ToString

            End With
        End If

        Return oBaseReceiptCashListType
    End Function

    ''' <summary>
    '''This method pass the request object to the stored procedure and the cover note is being added
    '''</summary>
    '''<param name="oGetReceiptCashListDetailsRequest" type="BaseGetReceiptCashListDetailsRequestType"></param>
    '''<param name="con" type="SiriusConnection"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.BaseImplementationTypes.BaseGetReceiptCashListDetailsResponseType</returns>
    '''<remarks></remarks>

    Public Overloads Function GetReceiptCashListDetails(ByVal con As SiriusConnection, ByVal oGetReceiptCashListDetailsRequest As BaseGetReceiptCashListDetailsRequestType) As BaseGetReceiptCashListDetailsResponseType

        Dim oGetReceiptCashListDetailsResponse As New BaseGetReceiptCashListDetailsResponseType
        Dim oCoreBusiness As New CoreBusiness
        Dim oErrors As New SAMErrorCollection
        Dim ibranchId As Integer
        Dim nTypeOfPackage As enumTypeOfPackage

        If oGetReceiptCashListDetailsRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.GetReceiptCashListDetailsRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oGetReceiptCashListDetailsResponse = New SAMForInsuranceV2ImplementationTypes.GetReceiptCashListDetailsResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If

        oGetReceiptCashListDetailsRequest.Validate(CObj(oErrors))
        oErrors.CheckForErrors()

        ' Lookup Codes to ensure they are valid – To be done for all codes in the request, e.g.
        ibranchId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
          PMLookupTable.Source, oGetReceiptCashListDetailsRequest.BranchCode, "Source", oErrors)
        oErrors.CheckForErrors()
        GetAndValidateSpecifiedTableCode(con, PMLookupTable.CashList, "cashlist_id", "cashlist_id", oGetReceiptCashListDetailsRequest.CashListKey.ToString(), oErrors, "CashListKey")
        oErrors.CheckForErrors()

        Dim oBaseRecieptCashListType As New BaseReceiptCashListType

        oBaseRecieptCashListType = CType(GetCashListforeceipt(con, oGetReceiptCashListDetailsRequest.CashListKey, CObj(oErrors)), BaseImplementationTypes.BaseReceiptCashListType)
        oErrors.CheckForErrors()

        If oBaseRecieptCashListType IsNot Nothing Then
            oGetReceiptCashListDetailsResponse.ReceiptCashList = New BaseReceiptCashListType

            oGetReceiptCashListDetailsResponse.ReceiptCashList.TypeCode = oBaseRecieptCashListType.TypeCode
            oGetReceiptCashListDetailsResponse.ReceiptCashList.ListDate = oBaseRecieptCashListType.ListDate
            oGetReceiptCashListDetailsResponse.ReceiptCashList.BankAccountCode = oBaseRecieptCashListType.BankAccountCode
            oGetReceiptCashListDetailsResponse.ReceiptCashList.CurrencyCode = oBaseRecieptCashListType.CurrencyCode
            oGetReceiptCashListDetailsResponse.ReceiptCashList.Reference = oBaseRecieptCashListType.Reference
            oGetReceiptCashListDetailsResponse.ReceiptCashList.StatusCode = oBaseRecieptCashListType.StatusCode
        End If
        Return oGetReceiptCashListDetailsResponse

    End Function

    ''' <summary>
    '''This method sends the request object values to the Stored Procedure and COM to Approve or decline the cash list item
    '''</summary>
    '''<param name="oApproveCashListItemRequest" type="BaseApproveCashListItemRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.BaseImplementationTypes.BaseApproveCashListItemResponseType</returns>
    '''<remarks></remarks>
    Public Overloads Function ApproveCashListItem(ByVal oApproveCashListItemRequest As BaseApproveCashListItemRequestType) As BaseApproveCashListItemResponseType
        Using conApproveCashListItem As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)
            Dim oResponse As BaseApproveCashListItemResponseType

            oResponse = ApproveCashListItem(conApproveCashListItem, oApproveCashListItemRequest)

            Return oResponse

        End Using

    End Function

    ''' <summary>
    '''This method sends the request object values to the Stored Procedure and COM to Approve or decline the cash list item
    '''</summary>
    '''<param name="conApproveCashListItem" type="SiriusConnection"></param>
    '''<param name="oApproveCashListItemRequest" type="BaseApproveCashListItemRequestType"></param>
    ''' <returns>An object of type SiriusFS.SAM.SFI.BaseImplementationTypes.BaseApproveCashListItemResponseType</returns>
    '''<remarks></remarks>
    Public Overloads Function ApproveCashListItem(ByVal conApproveCashListItem As SiriusConnection, ByVal oApproveCashListItemRequest As BaseApproveCashListItemRequestType) As BaseApproveCashListItemResponseType

        Dim oApproveCashListItemResponse As New BaseImplementationTypes.BaseApproveCashListItemResponseType
        Dim oCoreBusiness As New CoreBusiness
        Dim oErrors As New SAMErrorCollection

        Dim ibranchId As Integer
        Dim nTypeOfPackage As enumTypeOfPackage

        If oApproveCashListItemRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.ApproveCashListItemRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oApproveCashListItemResponse = New SAMForInsuranceV2ImplementationTypes.ApproveCashListItemResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If

        'Validate the mandatory parameters
        oApproveCashListItemRequest.Validate(CObj(oErrors))
        oErrors.CheckForErrors()

        ' Lookup Codes to ensure they are valid
        Try
            ibranchId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.Source, oApproveCashListItemRequest.BranchCode, "BranchCode")
        Catch ex As Exception
            oErrors.AddInvalidData(SAMInvalidData.InvalidLookupListValue,
                                        SAMInvalidData.InvalidLookupListValue.ToString,
                                        "BranchCode",
                                        oApproveCashListItemRequest.BranchCode)
        End Try

        ' exit if there are any missing parameters
        oErrors.CheckForErrors()

        Dim sResult As String
        Dim obusiness As New bACTCashListItem.Form
        SAMFunc.InitialiseSBOObject(conApproveCashListItem, obusiness, _SiriusUser, "bACTCashlistitem.Form")
        Dim oCashList As New bACTCashListPost.Automated 'As said by Rahul
        SAMFunc.InitialiseSBOObject(conApproveCashListItem, oCashList, _SiriusUser, "bACTCashListPost.Automated")
        Dim oStepAuthorization As New bACTCashListItem.StepAuthorization
        SAMFunc.InitialiseSBOObject(conApproveCashListItem, oStepAuthorization, _SiriusUser, "bACTCashlistitem.StepAuthorization")

        Try

            conApproveCashListItem.BeginTransaction()

            'Check product option for multi step approval (65). If error,set error data and exit. 
            sResult = oCoreBusiness.GetProductOption(SIRHiddenOptions.SIROPTMultiStepApproval, 1)
            oErrors.CheckForErrors()

            Dim dsCashListItem As New DataSet

            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_ACT_Select_CashListItem")

                cmd.AddInParameter("@cashlistitem_id", SqlDbType.Int).Value = oApproveCashListItemRequest.CashListItemKey
                dsCashListItem = conApproveCashListItem.ExecuteDataSet(cmd, "dtCashListItem")

            End Using

            If dsCashListItem.Tables.Count > 0 AndAlso dsCashListItem.Tables(0).Rows.Count > 0 Then
                oApproveCashListItemRequest.CashListKey = Convert.ToInt32(dsCashListItem.Tables(0).Rows(0).Item("CashList_ID"))
                oApproveCashListItemRequest.Amount = Convert.ToDecimal(dsCashListItem.Tables(0).Rows(0).Item("Amount"))
                oApproveCashListItemRequest.CreatorUserKey = Convert.ToInt32(dsCashListItem.Tables(0).Rows(0).Item("PMUser_ID"))
                oApproveCashListItemRequest.Reference = Convert.ToString(dsCashListItem.Tables(0).Rows(0).Item("our_ref"))
                oApproveCashListItemRequest.AccountKey = Convert.ToInt32(dsCashListItem.Tables(0).Rows(0).Item("Account_ID"))
            End If

            Dim dsCashList As New DataSet
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_ACT_Select_CashList")

                cmd.AddInParameter("@cashlist_id", SqlDbType.Int).Value = oApproveCashListItemRequest.CashListKey
                dsCashList = conApproveCashListItem.ExecuteDataSet(cmd, "dtCashList")

            End Using
            Dim lCashListTypeKey As Int32
            If dsCashList.Tables.Count > 0 AndAlso dsCashList.Tables(0).Rows.Count > 0 Then
                lCashListTypeKey = Convert.ToInt32(dsCashList.Tables(0).Rows(0).Item("CashListType_ID"))
            End If

            Dim icomReturnValue As Integer
            If sResult = "1" Then
                With oStepAuthorization
                    If lCashListTypeKey = 3 Then
                        .PaymentType = 1
                    Else
                        .PaymentType = 2
                    End If

                    .PaymentID = oApproveCashListItemRequest.CashListItemKey
                    .PaymentAmount = Cast.ToDecimal(oApproveCashListItemRequest.Amount, 0)
                    .PaymentCreatorUserID = oApproveCashListItemRequest.CreatorUserKey
                    .CashListSourceID = ibranchId
                End With

                If oApproveCashListItemRequest.Declined Then
                    icomReturnValue = oStepAuthorization.ProcessDecline()
                    If icomReturnValue <> PMEReturnCode.PMTrue AndAlso icomReturnValue <> PMEReturnCode.PMNotFound Then
                        RaiseComMethodException("bACTCashlistitem.StepAuthorization.ProcessDecline", icomReturnValue)
                    End If
                Else
                    icomReturnValue = oStepAuthorization.ProcessApproval()
                    If icomReturnValue <> PMEReturnCode.PMTrue AndAlso icomReturnValue <> PMEReturnCode.PMNotFound Then
                        RaiseComMethodException("bACTCashlistitem.StepAuthorization.ProcessApproval", icomReturnValue)
                    End If
                End If

                If oStepAuthorization.ProcessErrorMessage <> "" Then
                    Dim dsIsUserUnique As New DataSet

                    If Left(Trim(oStepAuthorization.ProcessErrorMessage), 6) Like "Debtor" Then
                        oErrors.AddInvalidData(SAMInvalidData.DebtorUserGroupsAreNotSetup,
                                                  SAMInvalidData.DebtorUserGroupsAreNotSetup.ToString,
                                                     oStepAuthorization.ProcessErrorMessage)
                    Else
                        oErrors.AddInvalidData(SAMInvalidData.UserUnconfirmedExceptions,
                                                  SAMInvalidData.UserUnconfirmedExceptions.ToString,
                                                     oStepAuthorization.ProcessErrorMessage)
                    End If
                    oErrors.CheckForErrors()
                    Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_Check_Is_User_Unique")
                        cmd.AddInParameter("@UserID", SqlDbType.Int).Value = _SiriusUser.UserID
                        cmd.AddInParameter("@PaymentID", SqlDbType.Int).Value = oApproveCashListItemRequest.CashListItemKey
                        dsIsUserUnique = conApproveCashListItem.ExecuteDataSet(cmd, "dsIsUserUnique")
                    End Using
                    If dsIsUserUnique IsNot Nothing AndAlso dsIsUserUnique.Tables.Count > 0 AndAlso dsIsUserUnique.Tables(0).Rows.Count > 0 Then
                        oErrors.AddInvalidData(SAMInvalidData.SameUserCanNotAuthoriseTwoSteps,
                                                  SAMInvalidData.SameUserCanNotAuthoriseTwoSteps.ToString,
                                                     oStepAuthorization.ProcessErrorMessage)

                    Else
                        RaiseComMethodException("bACTCashlistitem.StepAuthorization.ProcessErrorMessage" & oStepAuthorization.ProcessErrorMessage)
                    End If
                    oErrors.CheckForErrors()
                End If
                If oApproveCashListItemRequest.CheckValidationOnly Then
                    If conApproveCashListItem.TransactionCount > 0 Then
                        conApproveCashListItem.RollbackTransaction()
                    End If
                    Return oApproveCashListItemResponse
                End If
                icomReturnValue = obusiness.ProcessWTM(v_lCashListItemID:=oApproveCashListItemRequest.CashListItemKey)
                If icomReturnValue <> PMEReturnCode.PMTrue AndAlso icomReturnValue <> PMEReturnCode.PMNotFound Then
                    RaiseComMethodException("bACTCashlistitem.Form.ProcessWTM", icomReturnValue)
                End If

                Dim UserGroupKey As Integer
                icomReturnValue = obusiness.GetUserGroupID(v_lUserId:=oApproveCashListItemRequest.CreatorUserKey, r_lUserGroupID:=UserGroupKey)
                If icomReturnValue <> PMEReturnCode.PMTrue AndAlso icomReturnValue <> PMEReturnCode.PMNotFound Then
                    RaiseComMethodException("bACTCashlistitem.Form.GetUserGroupID", icomReturnValue)
                End If

                Dim suserGroupCode As String
                Dim saccountCode As String
                suserGroupCode = String.Empty
                saccountCode = String.Empty
                icomReturnValue = obusiness.GetAccountAndUserGroupCode(v_lAccountID:=oApproveCashListItemRequest.AccountKey, v_lUserGroupID:=UserGroupKey, r_sAccountCode:=saccountCode, r_sUsergroupCode:=suserGroupCode)
                If icomReturnValue <> PMEReturnCode.PMTrue AndAlso icomReturnValue <> PMEReturnCode.PMNotFound Then
                    RaiseComMethodException("bACTCashlistitem.Form.GetAccountAndUserGroupCode", icomReturnValue)
                End If

                Dim staskDesc As String
                staskDesc = String.Empty
                icomReturnValue = obusiness.GetCashListType(v_lCashListTypeID:=lCashListTypeKey, r_sCashListType:=staskDesc)
                If icomReturnValue <> PMEReturnCode.PMTrue AndAlso icomReturnValue <> PMEReturnCode.PMNotFound Then
                    RaiseComMethodException("bACTCashlistitem.Form.GetCashListType", icomReturnValue)
                End If

                Dim staskDescComplete As String
                staskDescComplete = String.Empty
                Dim vKeyArray(1, 4) As Object
                Dim nCashListItemPayment_StatusId As Integer
                Dim sTaskGroupCode As String = String.Empty

                If oStepAuthorization.LastStep Then
                    If oApproveCashListItemRequest.Declined Then
                        icomReturnValue = obusiness.DeleteUserProperty(v_sPropertyName:="cashlistitem_payment_status_id", v_bDeleteAll:=False)
                        nCashListItemPayment_StatusId = 9
                        icomReturnValue = obusiness.UpdateUserProperty(v_sPropertyName:="cashlistitem_payment_status_id", v_vPropertyValue:=9)
                    Else
                        icomReturnValue = obusiness.DeleteUserProperty(v_sPropertyName:="cashlistitem_payment_status_id", v_bDeleteAll:=False)
                        nCashListItemPayment_StatusId = 1
                        icomReturnValue = obusiness.UpdateUserProperty(v_sPropertyName:="cashlistitem_payment_status_id", v_vPropertyValue:=1)
                    End If
                    Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_UpdateCashListItem_Payment_Status")
                        cmd.AddInParameter("@nCashListItem_ID", SqlDbType.Int).Value = oApproveCashListItemRequest.CashListItemKey
                        cmd.AddInParameter("@nCashListItemPayment_StatusId", SqlDbType.Int).Value = nCashListItemPayment_StatusId
                        conApproveCashListItem.ExecuteNonQuery(cmd)
                    End Using

                    If icomReturnValue <> PMEReturnCode.PMTrue AndAlso icomReturnValue <> PMEReturnCode.PMNotFound Then
                        RaiseComMethodException("bACTCashlistitem.Form.UpdateUserProperty", icomReturnValue)
                    End If

                Else
                    icomReturnValue = oStepAuthorization.GetStepGroupCode(r_sGroupCode:=suserGroupCode)
                    If icomReturnValue <> PMEReturnCode.PMTrue AndAlso icomReturnValue <> PMEReturnCode.PMNotFound Then
                        RaiseComMethodException("bACTCashlistitem.StepAuthorization.GetStepGroupCode", icomReturnValue)
                    End If

                End If

                'Authorization Comment 
                Dim Authorization_Comment As String
                Dim dsResultSet As DataSet
                Dim comment() As String
                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_CashListItem_AuthorizationComment_Get")
                    cmd.AddInParameter("@nCashListItem_Id", SqlDbType.Int).Value = oApproveCashListItemRequest.CashListItemKey
                    dsResultSet = conApproveCashListItem.ExecuteDataSet(cmd, "CashListItem")
                End Using
                If dsResultSet IsNot Nothing AndAlso dsResultSet.Tables.Count > 0 AndAlso dsResultSet.Tables(0).Rows.Count > 0 Then
                    Authorization_Comment = Convert.ToString(dsResultSet.Tables(0).Rows(0).Item("authorization_comment"))
                    comment = Authorization_Comment.Split(CChar("-"))
                End If

                If oApproveCashListItemRequest.Declined Then
                    staskDescComplete = "Your Payment of amount " & oApproveCashListItemRequest.Amount & " has been declined on " & System.DateTime.Now.Date

                    If (Authorization_Comment <> "") Then
                        staskDescComplete = staskDescComplete & ". Remarks: " & comment(0)
                    End If
                Else
                    If (Not oStepAuthorization.LastStep) Then
                        staskDescComplete = "Your Payment of amount " & oApproveCashListItemRequest.Amount & " has been approved and further assigned To " & suserGroupCode & " On " & System.DateTime.Now.Date
                    Else
                        staskDescComplete = "Your Payment of amount " & oApproveCashListItemRequest.Amount & " has been approved on " & System.DateTime.Now.Date
                    End If
                End If
              
                vKeyArray(0, 0) = "cashlistitem_id"
                vKeyArray(1, 0) = oApproveCashListItemRequest.CashListItemKey

                vKeyArray(0, 1) = "cashlist_id"
                vKeyArray(1, 1) = oApproveCashListItemRequest.CashListKey

                vKeyArray(0, 2) = "cashlisttype_id"
                vKeyArray(1, 2) = lCashListTypeKey

                If Not oStepAuthorization.LastStep Then

                    vKeyArray(0, 3) = "actionkey"
                    vKeyArray(1, 3) = "approve"

                    vKeyArray(0, 4) = "cash_list_item_mode"
                    vKeyArray(1, 4) = 2
                    sTaskGroupCode = "SLACS"
                Else
                    'used the common group because we don't want to display the start navigator while task will dispalyed in Portal in Work manager.
                    sTaskGroupCode = "COMMON"

                End If

                Dim lPMWrkTaskInstanceCnt As Integer
                Dim sCustomer As String
                Dim dtTaskDueDate As Date
                dtTaskDueDate = Now()
                sCustomer = GetAndValidateDescriptionById(conApproveCashListItem, "account", "short_code", "account_id", oApproveCashListItemRequest.AccountKey.ToString)
                If oStepAuthorization.LastStep Then
                    icomReturnValue = obusiness.AddTaskToWorkManager(r_lPMWrkTaskInstanceCnt:=lPMWrkTaskInstanceCnt, v_sCustomer:=sCustomer, v_sDescription:=staskDescComplete, v_dtTaskDueDate:=dtTaskDueDate, v_sTaskCode:="SHOWCLITEM", v_sTaskGroupCode:=sTaskGroupCode, v_iUserID:=Convert.ToInt16(oApproveCashListItemRequest.CreatorUserKey), v_vKeyArray:=vKeyArray)
                Else
                    icomReturnValue = obusiness.AddTaskToWorkManager(r_lPMWrkTaskInstanceCnt:=lPMWrkTaskInstanceCnt, v_sCustomer:=sCustomer, v_sDescription:=staskDescComplete, v_dtTaskDueDate:=dtTaskDueDate, v_sTaskCode:="SHOWCLITEM", v_sTaskGroupCode:=sTaskGroupCode, v_sUserGroupCode:=suserGroupCode, v_vKeyArray:=vKeyArray)
                End If

                If icomReturnValue <> PMEReturnCode.PMTrue AndAlso icomReturnValue <> PMEReturnCode.PMNotFound Then
                    RaiseComMethodException("bACTCashlistitem.Form.AddTaskToWorkManager", icomReturnValue)
                End If

                If oStepAuthorization.LastStep And oApproveCashListItemRequest.Declined = False Then
                    Dim sChequeProductionEnabled As String = ""
                    oCoreBusiness.GetSystemOption(oApproveCashListItemRequest.BranchCode, SystemOption.ChequeProductionEnabled,
                                      sChequeProductionEnabled)

                    If Not (String.IsNullOrEmpty(sChequeProductionEnabled)) AndAlso (Trim(sChequeProductionEnabled) = "1") Then
                        oCashList.ChequeProduction = True
                    End If
                    icomReturnValue = oCashList.PostUnallocatedCash(v_vCashListID:=oApproveCashListItemRequest.CashListKey, v_vCashListItemID:=oApproveCashListItemRequest.CashListItemKey)
                    If icomReturnValue <> PMEReturnCode.PMTrue AndAlso icomReturnValue <> PMEReturnCode.PMNotFound Then
                        RaiseComMethodException("bACTCashlistitem.Automated.PostUnallocatedCash", icomReturnValue)
                    End If

                    Const kAutoAllocateIfAbleOptionNumber As Integer = 5059
                    Dim sOptionValue As String = String.Empty
                    oCoreBusiness.GetSystemOption(oApproveCashListItemRequest.BranchCode,
                                              kAutoAllocateIfAbleOptionNumber,
                                              sOptionValue)

                    If sOptionValue IsNot Nothing _
                AndAlso sOptionValue <> String.Empty _
                AndAlso sOptionValue = "1" _
                AndAlso Not oApproveCashListItemRequest.Declined Then

                        'SPY needs to be approved irrespective of the errors below
                        conApproveCashListItem.CommitTransaction()

                        ' We will be calling CheckWriteOffAndExchangeRateGainLoss 
                        ' to fill the oAllocation.OtherAllocatingTrans object
                        ' Within that method we will set oAllocation.LeadAllocatingTrans.TransDetailKey = 0
                        ' No point calling the same SP twice
                        Dim oAllocation As New BaseAllocationType
                        oAllocation.BranchCode = oApproveCashListItemRequest.BranchCode
                        ' Set the Source Id to which SPY to be approved belongs to
                        oAllocation.SourceID = Convert.ToInt32(dsCashList.Tables(0).Rows(0).Item("company_id"))
                        oAllocation.AccountKey = oApproveCashListItemRequest.AccountKey
                        oAllocation.LeadAllocatingTrans = New BaseTransDetailType
                        oAllocation.LeadAllocatingTrans.TransDetailKey = 0
                        oAllocation.LeadAllocatingTrans.CashListItemKey = oApproveCashListItemRequest.CashListItemKey
                        oAllocation.LeadAllocatingTrans.Amount = oApproveCashListItemRequest.Amount
                        oAllocation.AutoAllocate = True
                        ProcessAllocation(oCoreBusiness,
                                      conApproveCashListItem,
                                      oAllocation,
                                      oErrors)
                        oErrors.CheckForErrors()
                    End If

                End If
            End If

            If oStepAuthorization.LastStep And oApproveCashListItemRequest.Declined = False Then
                Dim lTransDetailKey As Integer
                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_ACT_Select_CashListItem")

                    cmd.AddInParameter("@cashlistitem_id", SqlDbType.Int).Value = oApproveCashListItemRequest.CashListItemKey
                    dsCashListItem = conApproveCashListItem.ExecuteDataSet(cmd, "dtCashListItem")

                End Using

                If dsCashListItem.Tables.Count > 0 AndAlso dsCashListItem.Tables(0).Rows.Count > 0 Then
                    lTransDetailKey = Convert.ToInt32(dsCashListItem.Tables(0).Rows(0).Item("transdetail_id"))
                End If

                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_Get_Claim_Payment_Transactions_from_CashListItem")

                    cmd.AddInParameter("@cashlistitem_id", SqlDbType.Int).Value = oApproveCashListItemRequest.CashListItemKey
                    cmd.AddInParameter("@account_id", SqlDbType.Int).Value = oApproveCashListItemRequest.AccountKey
                    dsCashListItem = conApproveCashListItem.ExecuteDataSet(cmd, "dtCashListItem")

                End Using

                Dim iCount As Integer = 0
                Dim oUpdateAllocationRequest As SAMForInsuranceV2ImplementationTypes.UpdateAllocationRequestType
                If dsCashListItem.Tables.Count > 0 AndAlso dsCashListItem.Tables(0).Rows.Count > 0 Then
                    oUpdateAllocationRequest = New SAMForInsuranceV2ImplementationTypes.UpdateAllocationRequestType
                    oUpdateAllocationRequest.AccountKey = oApproveCashListItemRequest.AccountKey
                    oUpdateAllocationRequest.BranchCode = oApproveCashListItemRequest.BranchCode
                    oUpdateAllocationRequest.TransdetailKey = lTransDetailKey
                    oUpdateAllocationRequest.Amount = oApproveCashListItemRequest.Amount
                    oUpdateAllocationRequest.CashListItemKey = oApproveCashListItemRequest.CashListItemKey

                    ReDim oUpdateAllocationRequest.Allocation(dsCashListItem.Tables(0).Rows.Count - 1)
                    For Each oRow As DataRow In dsCashListItem.Tables(0).Rows
                        oUpdateAllocationRequest.Allocation(iCount) = New BaseImplementationTypes.BaseUpdateAllocationRequestTypeAllocation
                        oUpdateAllocationRequest.Allocation(iCount).AllocationTransdetailKey = Convert.ToInt32(oRow.Item("transdetail_id"))
                        oUpdateAllocationRequest.Allocation(iCount).AllocationAmount = Convert.ToDouble(oRow.Item("amount"))

                        Dim AnyError As STSErrorType
                        Dim bIsLocked As Boolean
                        Dim btimestamp As Byte = Nothing
                        AnyError = oCoreBusiness.GetTimestamp(conApproveCashListItem,
                                            oApproveCashListItemRequest.BranchCode,
                                            CoreBusiness.LockName.TransDetailKey,
                                            oUpdateAllocationRequest.Allocation(iCount).AllocationTransdetailKey,
                                             oUpdateAllocationRequest.Allocation(iCount).AllocationTimeStamp,
                                            bIsLocked)

                        iCount = iCount + 1
                    Next
                    UpdateAllocation(conApproveCashListItem, oUpdateAllocationRequest)
                End If

            End If

            If conApproveCashListItem.TransactionCount > 0 Then
                conApproveCashListItem.CommitTransaction()
            End If

            If oStepAuthorization.LastStep AndAlso oApproveCashListItemRequest.Declined = False Then
                oErrors.AddInvalidData(SAMInvalidData.TransactionAutoAllocated,
                                                 SAMInvalidData.TransactionAutoAllocated.ToString,
                                                    "Payment Transaction has been auto allocated")
                oErrors.CheckForErrors()
            End If
        Catch ex As Exception
            If conApproveCashListItem.TransactionCount > 0 Then
                conApproveCashListItem.RollbackTransaction()
            End If
            Throw

        Finally
            If obusiness IsNot Nothing Then
                obusiness.Dispose()
                obusiness = Nothing
            End If
            If oCashList IsNot Nothing Then
                oCashList.Dispose()
                oCashList = Nothing
            End If
            If oStepAuthorization IsNot Nothing Then
                oStepAuthorization.Dispose()
                oStepAuthorization = Nothing
            End If
        End Try

        Return oApproveCashListItemResponse

    End Function

    ''' <summary>
    ''' To get payment cash list details
    ''' </summary>
    '''<param name="con">An object of SiriusConnection class</param>
    '''<param name="icashListKey">An object of CashlistKey</param>   
    ''' <summary>
    Private Function GetCashListforPayment(ByVal con As SiriusConnection, ByVal icashListKey As Integer) As BaseCoreCashListType
        Dim dsCashList As New DataSet
        Dim oBusiness As New CoreBusiness
        Dim oBasePaymentCashListType As New BasePaymentCashListType
        Dim oErrors As New SAMErrorCollection

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_ACT_Select_CashList")
            cmd.AddInParameter("@cashlist_id", SqlDbType.Int).Value = Cast.NullIfDefault(icashListKey, Nothing)
            dsCashList = con.ExecuteDataSet(cmd, "CashList")
        End Using

        If dsCashList IsNot Nothing AndAlso dsCashList.Tables.Count > 0 AndAlso dsCashList.Tables(0).Rows.Count > 0 Then

            With oBasePaymentCashListType
                .TypeCode = oBusiness.GetListItemFromID(Core.STSListType.PMLookup, PMLookupTable.CashListType, Convert.ToInt32(dsCashList.Tables(0).Rows(0).Item("cashlisttype_id")))
                .BankAccountCode = oBusiness.GetListItemFromID(Core.STSListType.PMLookup, PMLookupTable.BankAccount, Convert.ToInt32(dsCashList.Tables(0).Rows(0).Item("bankaccount_id")))
                .CurrencyCode = oBusiness.GetListItemFromID(Core.STSListType.PMLookup, PMLookupTable.Currency, Convert.ToInt32(dsCashList.Tables(0).Rows(0).Item("currency_id")))
                .StatusCode = oBusiness.GetListItemFromID(Core.STSListType.PMLookup, PMLookupTable.CashListStatus, Convert.ToInt32(dsCashList.Tables(0).Rows(0).Item("cashliststatus_id")))
                .Reference = Cast.ToString(dsCashList.Tables(0).Rows(0).Item("cashlist_ref"), String.Empty)
                .ListDate = Cast.ToDateTime(dsCashList.Tables(0).Rows(0).Item("list_date"), Date.MinValue)
                .CashListKey = icashListKey
            End With
        End If

        Return oBasePaymentCashListType

    End Function

    ''' <summary>  
    ''' Create the cash list with payment items.
    ''' </summary>  
    ''' <param name="oCreatePaymentCashListWithItemsRequest">An object of type BaseCreatePaymentCashListWithItemsRequestType</param>  
    Public Overloads Function CreatePaymentCashListWithItems(ByVal oCreatePaymentCashListWithItemsRequest As BaseCreatePaymentCashListWithItemsRequestType) As BaseCreatePaymentCashListWithItemsResponseType
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)
            Dim oResponse As BaseCreatePaymentCashListWithItemsResponseType

            oResponse = CreatePaymentCashListWithItems(con, oCreatePaymentCashListWithItemsRequest)

            Return oResponse

        End Using

    End Function

    ''' <summary>  
    ''' Create CashList with Payment Items.
    ''' </summary>  
    ''' <param name="oCreatePaymentCashListWithItemsRequest">An object of type BaseCreatePaymentCashListWithItemsRequestType</param>  
    Public Overloads Function CreatePaymentCashListWithItems(ByVal con As SiriusConnection, ByVal oCreatePaymentCashListWithItemsRequest As BaseCreatePaymentCashListWithItemsRequestType) As BaseCreatePaymentCashListWithItemsResponseType

        Dim oCreatePaymentCashListWithItemsResponse As New BaseCreatePaymentCashListWithItemsResponseType
        Dim oCoreBusiness As New CoreBusiness
        Dim oErrors As New SAMErrorCollection
        Dim sBranchCode As String
        Dim nPartyKey As Integer = 0
        Dim sAccountShortCode As String = ""

        Dim nTypeOfPackage As enumTypeOfPackage
        Dim documentCode As String = String.Empty

        If oCreatePaymentCashListWithItemsRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.CreatePaymentCashListWithItemsRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oCreatePaymentCashListWithItemsResponse = New SAMForInsuranceV2ImplementationTypes.CreatePaymentCashListWithItemsResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If
        Dim SourceId As Integer
        If (Not String.IsNullOrEmpty(oCreatePaymentCashListWithItemsRequest.BranchCode)) Then
            SourceId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
                PMLookupTable.Source, oCreatePaymentCashListWithItemsRequest.BranchCode, "BranchCode", oErrors)
        End If
        oCreatePaymentCashListWithItemsRequest.PaymentCashList.SourceID = SourceId
        sBranchCode = oCreatePaymentCashListWithItemsRequest.BranchCode

        'Validate the mandatory parameters
        oCreatePaymentCashListWithItemsRequest.Validate(CType(oErrors, Object))
        oErrors.CheckForErrors()

        ValidateCashList(con, oCoreBusiness, DirectCast(oCreatePaymentCashListWithItemsRequest.PaymentCashList, BaseCoreCashListType), oErrors)
        oErrors.CheckForErrors()

        sBranchCode = oCreatePaymentCashListWithItemsRequest.BranchCode

        Dim iDocumentId As Integer = 0
        Dim sDocumentRef As String
        If (oCreatePaymentCashListWithItemsRequest.PaymentCashList.CashListKey <> 0) Then
            'Currently, this application supports creation of new cash list, not modification of existing cash list.
            'If oCashList.CashListKey is not zero then the cashlist is not a new cash list.
            con.BeginTransaction()
            Try
                Dim nRecordCount As Integer = 0
                If oCreatePaymentCashListWithItemsRequest.PaymentCashList.PaymentItem IsNot Nothing Then
                    For Each oCashListItem As BasePaymentCashListItemType In oCreatePaymentCashListWithItemsRequest.PaymentCashList.PaymentItem
                        oCashListItem.BranchCode = sBranchCode
                        ProcessCashListItem(con, oCoreBusiness, DirectCast(oCreatePaymentCashListWithItemsRequest.PaymentCashList, BaseCoreCashListType), DirectCast(oCashListItem, BaseCoreCashListItemType), oErrors, False, 0, bCreateCashListItem:=False)
                        ReDim Preserve oCreatePaymentCashListWithItemsResponse.CashListItem(nRecordCount)
                        oCreatePaymentCashListWithItemsResponse.CashListItem(nRecordCount) = New BaseCreatePaymentCashListWithItemsResponseTypeCashListItem
                        sAccountShortCode = oCreatePaymentCashListWithItemsRequest.PaymentCashList.PaymentItem(nRecordCount).AccountShortCode
                        oCreatePaymentCashListWithItemsResponse.CashListItem(nRecordCount).AccountShortCode = sAccountShortCode
                        oCreatePaymentCashListWithItemsResponse.CashListItem(nRecordCount).CashListItemKey = oCreatePaymentCashListWithItemsRequest.PaymentCashList.PaymentItem(nRecordCount).CashListItemKey
                        oCreatePaymentCashListWithItemsResponse.CashListItem(nRecordCount).TransDetailKey = oCreatePaymentCashListWithItemsRequest.PaymentCashList.PaymentItem(nRecordCount).TransDetailKey
                        nRecordCount += 1

                        oErrors.CheckForErrors()
                        If oCashListItem.IsProduceDocument Then
                            nPartyKey = GetAndValidateSpecifiedTableCode(con,
                                                PMLookupTable.Party, "Party_cnt", "shortname", sAccountShortCode, oErrors, sAccountShortCode.ToString())
                            If oCashListItem.TransDetailKey <> 0 Then
                                iDocumentId = GetAndValidateSpecifiedTableCode(con,
                                           "transdetail", "document_id", "transdetail_id", oCashListItem.TransDetailKey.ToString(), oErrors, "")

                                If iDocumentId <> 0 Then
                                    sDocumentRef = GetAndValidateDescriptionById(con, "document", "document_ref", "document_id", iDocumentId.ToString())
                                    oCreatePaymentCashListWithItemsResponse.CashListItem(nRecordCount).DocumentRef = sDocumentRef
                                    GenerateCashChequeReceiptAndPaymentDocument(con:=con, v_sBranchCode:=sBranchCode,
                                                                                     v_nSourceId:=SourceId, v_nPartyKey:=nPartyKey, v_sType:="P", v_sDocumentRef:=sDocumentRef, v_sDocumentCode:=oCreatePaymentCashListWithItemsResponse.CashListItem(nRecordCount).DocumentCode)
                                End If
                            End If
                        End If
                    Next
                End If
                oCreatePaymentCashListWithItemsResponse.CashListKey = oCreatePaymentCashListWithItemsRequest.PaymentCashList.CashListKey
                con.CommitTransaction()
            Catch ex As Exception
                con.RollbackTransaction()
                Throw
            End Try

        Else

            con.BeginTransaction()
            Dim oCoreCashList As New BaseCoreCashListType
            Dim dAmount As Double = 0
            Try

                oCoreCashList = DirectCast(oCreatePaymentCashListWithItemsRequest.PaymentCashList, BaseCoreCashListType)

                CreateCashList(con, oCoreCashList, oErrors)
                oErrors.CheckForErrors()
                oCreatePaymentCashListWithItemsResponse.CashListKey = oCoreCashList.CashListKey

                Dim bAutoAllocatePaymentSuccessful As Boolean = True
                Dim iRecordCount As Integer = 0
                Dim shAllcationStatus As Short

                If oCreatePaymentCashListWithItemsRequest.PaymentCashList.PaymentItem IsNot Nothing Then
                    For Each oCashListItem As BasePaymentCashListItemType In oCreatePaymentCashListWithItemsRequest.PaymentCashList.PaymentItem
                        oCashListItem.BranchCode = sBranchCode
                        ProcessCashListItem(con, oCoreBusiness,
                                            DirectCast(oCreatePaymentCashListWithItemsRequest.PaymentCashList, BaseCoreCashListType),
                                            DirectCast(oCashListItem, BaseCoreCashListItemType),
                                            oErrors,
                                            bAutoAllocatePaymentSuccessful:=bAutoAllocatePaymentSuccessful)
                        ReDim Preserve oCreatePaymentCashListWithItemsResponse.CashListItem(iRecordCount)
                        oCreatePaymentCashListWithItemsResponse.CashListItem(iRecordCount) = New BaseCreatePaymentCashListWithItemsResponseTypeCashListItem
                        sAccountShortCode = oCreatePaymentCashListWithItemsRequest.PaymentCashList.PaymentItem(iRecordCount).AccountShortCode
                        oCreatePaymentCashListWithItemsResponse.CashListItem(iRecordCount).AccountShortCode = sAccountShortCode
                        oCreatePaymentCashListWithItemsResponse.CashListItem(iRecordCount).CashListItemKey = oCreatePaymentCashListWithItemsRequest.PaymentCashList.PaymentItem(iRecordCount).CashListItemKey
                        oCreatePaymentCashListWithItemsResponse.CashListItem(iRecordCount).TransDetailKey = oCreatePaymentCashListWithItemsRequest.PaymentCashList.PaymentItem(iRecordCount).TransDetailKey
                        oCreatePaymentCashListWithItemsResponse.CashListItem(iRecordCount).AutoAllocatePaymentSuccessful = bAutoAllocatePaymentSuccessful

                        If oCashListItem.Policies IsNot Nothing Then
                            For Each PolicyItem As BasePaymentCashListItemTypePolicies In oCashListItem.Policies
                                AllocatePaymentsforPolicies(con, PolicyItem, oCashListItem, oCreatePaymentCashListWithItemsResponse.CashListKey, shAllcationStatus)
                            Next
                        End If


                        oErrors.CheckForErrors()
                        If oCashListItem.IsProduceDocument Then
                            nPartyKey = GetAndValidateSpecifiedTableCode(con,
                                                PMLookupTable.Party, "Party_cnt", "shortname", sAccountShortCode, oErrors, sAccountShortCode.ToString())
                            If oCashListItem.TransDetailKey <> 0 Then
                                iDocumentId = GetAndValidateSpecifiedTableCode(con,
                                                                        "transdetail", "document_id", "transdetail_id", oCashListItem.TransDetailKey.ToString(), oErrors, "")

                                If iDocumentId <> 0 Then
                                    sDocumentRef = GetAndValidateDescriptionById(con, "document", "document_ref", "document_id", iDocumentId.ToString())
                                    oCreatePaymentCashListWithItemsResponse.CashListItem(iRecordCount).DocumentRef = sDocumentRef
                                    GenerateCashChequeReceiptAndPaymentDocument(con:=con, v_sBranchCode:=sBranchCode,
                                                                                         v_nSourceId:=SourceId, v_nPartyKey:=nPartyKey, v_sType:="P", v_sDocumentRef:=sDocumentRef, v_sDocumentCode:=oCreatePaymentCashListWithItemsResponse.CashListItem(iRecordCount).DocumentCode)
                                End If
                            End If

                        End If
                        iRecordCount += 1
                    Next
                End If
                'oCreatePaymentCashListWithItemsResponse.CashListKey = oCoreCashList.CashListKey
                con.CommitTransaction()
            Catch ex As Exception
                con.RollbackTransaction()
                Throw
            Finally
                oCoreBusiness = Nothing
                oCoreCashList = Nothing
            End Try
        End If

        Return oCreatePaymentCashListWithItemsResponse

    End Function

    ''' <summary>  
    ''' Create the cash list item.
    ''' </summary>  
    ''' <param name="oCreatePaymentCashListItemRequest">An object of type SiriusFS.SAM.BaseImplementationTypes.BaseCreatePaymentCashListItemRequestType</param>  
    Public Overloads Function CreatePaymentCashListItem(ByVal oCreatePaymentCashListItemRequest As BaseCreatePaymentCashListItemRequestType) As BaseCreatePaymentCashListItemResponseType
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)
            Dim oResponse As BaseCreatePaymentCashListItemResponseType

            oResponse = CreatePaymentCashListItem(con, oCreatePaymentCashListItemRequest)

            Return oResponse

        End Using

    End Function
    ''' <summary>  
    ''' Create CashList Payment Item.
    ''' </summary>  
    ''' <param name="oCreatePaymentCashListItemRequest">An object of type SiriusFS.SAM.BaseImplementationTypes.BaseCreatePaymentCashListItemRequestType</param>  

    Public Overloads Function CreatePaymentCashListItem(ByVal con As SiriusConnection, ByVal oCreatePaymentCashListItemRequest As BaseCreatePaymentCashListItemRequestType) As BaseCreatePaymentCashListItemResponseType

        Dim oCreatePaymentCashListItemResponse As New BaseCreatePaymentCashListItemResponseType
        Dim oCoreBusiness As New CoreBusiness
        Dim oErrors As New SAMErrorCollection

        Dim branchId As Integer
        Dim nTypeOfPackage As enumTypeOfPackage

        If oCreatePaymentCashListItemRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.CreatePaymentCashListItemRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage
            oCreatePaymentCashListItemResponse = New SAMForInsuranceV2ImplementationTypes.CreatePaymentCashListItemResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If

        ' exit if there are any missing parameters

        'Validate the mandatory parameters
        oCreatePaymentCashListItemRequest.Validate(CType(oErrors, Object))
        oErrors.CheckForErrors()
        Dim oCashList As New BaseCoreCashListType

        oCashList = GetCashListforPayment(con, oCreatePaymentCashListItemRequest.CashListKey)
        ' Lookup Codes to ensure they are valid – To be done for all codes in the request, e.g.

        Try
            branchId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.Source, oCreatePaymentCashListItemRequest.BranchCode, "BranchCode")
        Catch ex As Exception
            oErrors.AddInvalidData(SAMInvalidData.InvalidLookupListValue,
                                    SAMInvalidData.InvalidLookupListValue.ToString,
                                    "BranchCode",
                                    oCreatePaymentCashListItemRequest.BranchCode)
        End Try
        oErrors.CheckForErrors()

        con.BeginTransaction()
        If oCreatePaymentCashListItemRequest.PaymentItem IsNot Nothing Then
            Try

                Dim oCoreCashListItem As BaseCoreCashListItemType
                For Each oCashListItem As BasePaymentCashListItemType In oCreatePaymentCashListItemRequest.PaymentItem
                    oCoreCashListItem = DirectCast(oCashListItem, BaseCoreCashListItemType)

                    ValidateCashListItem(con, oCoreBusiness, oCashList, oCoreCashListItem, oErrors)
                    oErrors.CheckForErrors()

                    CreateCashListItem(con, oCoreCashListItem, oCashList, oErrors)
                    oErrors.CheckForErrors()

                    PostCashListItem(con, oCoreCashListItem, oCashList.CashListKey, oErrors)
                    oErrors.CheckForErrors()

                    If oCreatePaymentCashListItemResponse.CashListItemKey Is Nothing Then
                        ReDim oCreatePaymentCashListItemResponse.CashListItemKey(0)

                        ReDim oCreatePaymentCashListItemResponse.TransDetailKey(0)

                    Else
                        ReDim oCreatePaymentCashListItemResponse.CashListItemKey(oCreatePaymentCashListItemResponse.CashListItemKey.Length)

                        ReDim oCreatePaymentCashListItemResponse.TransDetailKey(oCreatePaymentCashListItemResponse.TransDetailKey.Length)

                    End If

                    oCreatePaymentCashListItemResponse.CashListItemKey(oCreatePaymentCashListItemResponse.CashListItemKey.Length - 1) = oCashListItem.CashListItemKey
                    '        Start (Sriram P)As per the gap  analysis on 19082008 as confirmted  by rahul

                    oCreatePaymentCashListItemResponse.TransDetailKey(oCreatePaymentCashListItemResponse.TransDetailKey.Length - 1) = oCashListItem.TransDetailKey

                    oCoreCashListItem = Nothing
                Next
                con.CommitTransaction()
                Return oCreatePaymentCashListItemResponse

            Catch ex As Exception
                con.RollbackTransaction()
                Return oCreatePaymentCashListItemResponse

            End Try
        End If

        Return oCreatePaymentCashListItemResponse

    End Function

    ' Start (Udaya Bhaskar K ) -(Tech Spec - UIIC WR62 - Cash Cheque Receipt - Create Receipt Cash List) -(7.1.4.5) 
    ''' <summary>
    ''' Get a Reciept cashlist item for this payment
    ''' </summary>
    '''<param name="oGetReceiptCashListItemDetailsRequest" type ="BaseGetReceiptCashListItemDetailsRequestType"></param>
    '''<returns>"oResponse" type ="BaseGetReceiptCashListItemDetailsResponseType"</returns>
    '''</summary>
    Public Overloads Function GetReceiptCashListItemDetails(ByVal oGetReceiptCashListItemDetailsRequest As BaseGetReceiptCashListItemDetailsRequestType) As BaseGetReceiptCashListItemDetailsResponseType
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)
            Dim oResponse As BaseGetReceiptCashListItemDetailsResponseType

            oResponse = GetReceiptCashListItemDetails(con, oGetReceiptCashListItemDetailsRequest)

            Return oResponse

        End Using

    End Function

    ' Start (Udaya Bhaskar K ) -(Tech Spec - UIIC WR62 - Cash Cheque Receipt - Create Receipt Cash List) -(7.1.4.5) 
    ''' <summary>
    ''' Get a Reciept cashlist item for this payment
    ''' </summary>
    '''<param name="con" type ="SiriusConnection"></param>
    '''<param name="oGetReceiptCashListItemDetailsRequest" type ="BaseGetReceiptCashListItemDetailsRequestType"></param>
    '''<returns>"oResponse" type ="BaseGetReceiptCashListItemDetailsResponseType"</returns>
    '''</summary>
    Public Overloads Function GetReceiptCashListItemDetails(ByVal con As SiriusConnection, ByVal oGetReceiptCashListItemDetailsRequest As BaseGetReceiptCashListItemDetailsRequestType) As BaseGetReceiptCashListItemDetailsResponseType

        Dim oGetReceiptCashListItemDetailsResponse As New BaseGetReceiptCashListItemDetailsResponseType
        Dim oCoreBusiness As New CoreBusiness
        Dim oErrors As New SAMErrorCollection
        Dim dsReceipt As DataSet
        Dim ibranchId As Integer
        Dim nTypeOfPackage As enumTypeOfPackage

        If oGetReceiptCashListItemDetailsRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.GetReceiptCashListItemDetailsRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage
            oGetReceiptCashListItemDetailsResponse = New SAMForInsuranceV2ImplementationTypes.GetReceiptCashListItemDetailsResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If

        'Validate the mandatory parameters
        oGetReceiptCashListItemDetailsRequest.Validate(CType(oErrors, Object))
        oErrors.CheckForErrors()

        Try
            ibranchId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.Source, oGetReceiptCashListItemDetailsRequest.BranchCode, "BranchCode")
        Catch ex As Exception
            oErrors.AddInvalidData(SAMInvalidData.InvalidLookupListValue,
                                        SAMInvalidData.InvalidLookupListValue.ToString,
                                        "BranchCode",
                                        oGetReceiptCashListItemDetailsRequest.BranchCode)
        End Try

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_ACT_Select_CashListItem")
            cmd.AddInParameter("@cashlistitem_id", SqlDbType.Int).Value = Cast.NullIfDefault(oGetReceiptCashListItemDetailsRequest.CashListItemKey)

            dsReceipt = con.ExecuteDataSet(cmd, "Receipt")

        End Using

        'Filling up the Response object
        If dsReceipt IsNot Nothing AndAlso dsReceipt.Tables.Count > 0 AndAlso dsReceipt.Tables(0).Rows.Count > 0 Then

            oGetReceiptCashListItemDetailsResponse.CashListReceipt = New BaseImplementationTypes.BaseReceiptCashListItemType
            With oGetReceiptCashListItemDetailsResponse.CashListReceipt

                .IsProduceDocument = Cast.ToBoolean(dsReceipt.Tables(0).Rows(0).Item("letter"), False)

                .AccountKey = Cast.ToInt32(dsReceipt.Tables(0).Rows(0).Item("account_id"), 0)
                .AccountShortCode = GetAndValidateDescriptionById(con, "Account", "short_code", "account_id", .AccountKey.ToString())
                .AllocationStatusKey = Cast.ToInt32(dsReceipt.Tables(0).Rows(0).Item("allocationstatus_id"), 0)
                .AllocationStatusCode = GetAndValidateDescriptionById(con, "AllocationStatus", "code", "allocationstatus_id", .AllocationStatusKey.ToString())
                .TypeKey = Cast.ToInt32(dsReceipt.Tables(0).Rows(0).Item("cashlistitem_receipt_type_id"), 0)
                .TypeCode = GetAndValidateDescriptionById(con, "cashlistitem_receipt_type", "code", "cashlistitem_receipt_type_id", .TypeKey.ToString())
                .StatusKey = Cast.ToInt32(dsReceipt.Tables(0).Rows(0).Item("cashlistitem_receipt_status_id"), 0)

                '.StatusCode = GetAndValidateDescriptionById(con, "cashliststatus", "code", "cashliststatus_id", .StatusKey.ToString())
                .StatusCode = GetAndValidateDescriptionById(con, "cashlistitem_receipt_status", "code", "cashlistitem_receipt_status_id", .StatusKey.ToString())
                .MediaTypeKey = Cast.ToInt32(dsReceipt.Tables(0).Rows(0).Item("mediatype_id"), 0)
                .MediaTypeCode = GetAndValidateDescriptionById(con, "mediatype", "code", "mediatype_id", .MediaTypeKey.ToString())
                .Amount = Cast.ToDecimal(dsReceipt.Tables(0).Rows(0).Item("amount"), 0)
                .TaxBandCode = GetAndValidateDescriptionById(con, "tax_band", "code", "tax_band_id", Cast.ToString(dsReceipt.Tables(0).Rows(0).Item("tax_band_id"), String.Empty))

                'Filling Bank 
                oGetReceiptCashListItemDetailsResponse.CashListReceipt.Bank = New BaseImplementationTypes.BaseBankReceiptType
                .BankKey = Cast.ToInt32(dsReceipt.Tables(0).Rows(0).Item("cashlistitem_bank_id"), 0)
                .Bank.BankKey = Cast.ToInt32(dsReceipt.Tables(0).Rows(0).Item("cashlistitem_bank_id"), 0)
                .Bank.BankCode = GetAndValidateDescriptionById(con, "cashlistitem_bank", "code", "cashlistitem_bank_id", .BankKey.ToString())
                .Bank.ChequeDate = Cast.ToDateTime(dsReceipt.Tables(0).Rows(0).Item("cheque_date"), Date.MinValue)
                .Bank.PayerName = Cast.ToString(dsReceipt.Tables(0).Rows(0).Item("payment_name"), String.Empty)

                '.CashListItemKey = Cast.ToInt32(dsReceipt.Tables(0).Rows(0).Item("cashlistitem_id"), 0)

                'Filling of Contacts
                oGetReceiptCashListItemDetailsResponse.CashListReceipt.ContactAddress = New BaseImplementationTypes.BaseSimpleAddressType
                .ContactAddress.AddressLine1 = Cast.ToString(dsReceipt.Tables(0).Rows(0).Item("address1"), String.Empty)
                .ContactAddress.AddressLine2 = Cast.ToString(dsReceipt.Tables(0).Rows(0).Item("address2"), String.Empty)
                .ContactAddress.AddressLine3 = Cast.ToString(dsReceipt.Tables(0).Rows(0).Item("address3"), String.Empty)
                .ContactAddress.AddressLine4 = Cast.ToString(dsReceipt.Tables(0).Rows(0).Item("address4"), String.Empty)
                .ContactAddress.CountryKey = Cast.ToInt32(dsReceipt.Tables(0).Rows(0).Item("address_country"), 0)
                .ContactAddress.CountryCode = GetAndValidateDescriptionById(con, "Country", "code", "country_id", .ContactAddress.CountryKey.ToString())
                .ContactAddress.PostCode = Cast.ToString(dsReceipt.Tables(0).Rows(0).Item("postal_code"), String.Empty)
                .ContactName = Cast.ToString(dsReceipt.Tables(0).Rows(0).Item("contact_name"), String.Empty)

                'Filling of CreditCard info
                oGetReceiptCashListItemDetailsResponse.CashListReceipt.CreditCard = New BaseImplementationTypes.BaseCreditCardType
                .CreditCard.Number = Cast.ToString(dsReceipt.Tables(0).Rows(0).Item("cc_number"), String.Empty)
                .CreditCard.AuthCode = Cast.ToString(dsReceipt.Tables(0).Rows(0).Item("cc_auth_code"), String.Empty)
                '.CreditCard.CardHolder.
                If (dsReceipt.Tables(0).Rows(0).Item("cc_customer") Is String.Empty) Then
                    .CreditCard.CustomerPresent = False
                Else
                    .CreditCard.CustomerPresent = True
                End If
                .CreditCard.ExpiryDate = Cast.ToString(dsReceipt.Tables(0).Rows(0).Item("cc_expiry_date"), String.Empty)
                .CreditCard.StartDate = Cast.ToString(dsReceipt.Tables(0).Rows(0).Item("cc_start_date"), String.Empty)
                .CreditCard.Issue = Cast.ToString(dsReceipt.Tables(0).Rows(0).Item("cc_issue"), String.Empty)

                .CreditCard.ManualAuthCode = Cast.ToString(dsReceipt.Tables(0).Rows(0).Item("cc_manual_auth_code"), String.Empty)
                .CreditCard.NameOnCreditCard = Cast.ToString(dsReceipt.Tables(0).Rows(0).Item("cc_name"), String.Empty)
                .CreditCard.Pin = Cast.ToString(dsReceipt.Tables(0).Rows(0).Item("cc_pin"), String.Empty)
                .CreditCard.TransactionCode = Cast.ToString(dsReceipt.Tables(0).Rows(0).Item("cc_transaction_code"), String.Empty)
                '.CreditCard.TypeCode = GetAndValidateDescriptionById(con, "mediatype", "code", "mediatype_id", .MediaTypeKey.ToString())

                .FurtherDetails = Cast.ToString(dsReceipt.Tables(0).Rows(0).Item("receipt_details"), String.Empty)
                '.isValidated = Cast.ToBoolean(GetAndValidateDescriptionById(con, "mediatype", "code", "mediatype_id", .MediaTypeKey.ToString()), False)
                .MediaReference = Cast.ToString(dsReceipt.Tables(0).Rows(0).Item("Media_ref"), String.Empty)
                .OurReference = Cast.ToString(dsReceipt.Tables(0).Rows(0).Item("Our_ref"), String.Empty)

                .TheirReference = Cast.ToString(dsReceipt.Tables(0).Rows(0).Item("their_ref"), String.Empty)
                .TransactionDate = Cast.ToDateTime(dsReceipt.Tables(0).Rows(0).Item("Transaction_date"), DateTime.MinValue)
                '.TransDetailKey = Cast.ToInt32(dsReceipt.Tables(0).Rows(0).Item("transdetail_id"), 0)

            End With
        End If

        Return oGetReceiptCashListItemDetailsResponse
    End Function

    ''' <summary>
    '''  This method will call another method to do the business
    ''' </summary>
    '''<param name="oCreateReceiptCashListItemsRequest">An object of the class BaseCreateReceiptCashListItemRequestType </param>

    Public Overloads Function CreateReceiptCashListItem(ByVal oCreateReceiptCashListItemsRequest As BaseCreateReceiptCashListItemRequestType) As BaseCreateReceiptCashListItemResponseType
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                               _SiriusUser.Username, _SiriusUser.SourceID,
                                               _SiriusUser.LanguageID,
                                               SiriusUserDefaults.AppName)
            Dim oResponse As BaseCreateReceiptCashListItemResponseType

            oResponse = CreateReceiptCashListItem(con, oCreateReceiptCashListItemsRequest)

            Return oResponse

        End Using

    End Function

    ''' <summary>
    '''  This method will call another method to do the business
    ''' </summary>
    '''<param name="oCreateReceiptCashListItemsRequest">An object of the class BaseCreateReceiptCashListItemRequestType </param>
    '''<param name="conCashlistItem">An object of the class SiriusConnection </param>

    Public Overloads Function CreateReceiptCashListItem(ByVal conCashlistItem As SiriusConnection, ByVal oCreateReceiptCashListItemsRequest As BaseCreateReceiptCashListItemRequestType) As BaseCreateReceiptCashListItemResponseType
        Dim oCreateReceiptCashListItemsResponse As New BaseCreateReceiptCashListItemResponseType
        Dim oSAMErrorCollection As New SAMErrorCollection
        Dim nTypeOfPackage As enumTypeOfPackage
        Dim oCoreBusiness As New CoreBusiness

        'Dim aArrayList As New ArrayList

        If oCreateReceiptCashListItemsRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.CreateReceiptCashListItemRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oCreateReceiptCashListItemsResponse = New SAMForInsuranceV2ImplementationTypes.CreateReceiptCashListItemResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If

        '**********************************************************************************************
        ''Structure Validation(Mandatory Checking)
        '**********************************************************************************************
        If (oCreateReceiptCashListItemsRequest IsNot Nothing) Then

            oCreateReceiptCashListItemsRequest.Validate(CObj(oSAMErrorCollection))

            If (oCreateReceiptCashListItemsRequest.ReceiptCashListItem IsNot Nothing) Then
                For Each ReceiptItem As BaseReceiptCashListItemType In oCreateReceiptCashListItemsRequest.ReceiptCashListItem
                    ReceiptItem.Validate(CObj(oSAMErrorCollection))
                Next
            End If
            oSAMErrorCollection.CheckForErrors()

            '**********************************************************************************************
            'Data Validation
            '**********************************************************************************************
            oCreateReceiptCashListItemsRequest.SourceId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
               PMLookupTable.Source, oCreateReceiptCashListItemsRequest.BranchCode, "BranchCode", oSAMErrorCollection)

            If (oCreateReceiptCashListItemsRequest.ReceiptCashListItem IsNot Nothing) Then
                For Each ReceiptItem As BaseReceiptCashListItemType In oCreateReceiptCashListItemsRequest.ReceiptCashListItem
                    'Arul Start
                    ValidateBaseReceiptCashListItemType(oCoreBusiness, conCashlistItem, DirectCast(GetCashListForReceipt(conCashlistItem, oCreateReceiptCashListItemsRequest.CashListKey), BaseReceiptCashListType), DirectCast(ReceiptItem, BaseReceiptCashListItemType), oSAMErrorCollection)
                Next
                oSAMErrorCollection.CheckForErrors()
                Try
                    conCashlistItem.BeginTransaction()
                    Dim iRecordCount As Integer = 0
                    Dim iCount As Integer = 0
                    Dim shAllcationStatus As Short
                    Dim iPartyKey As Integer
                    Dim dsPolicies As DataSet
                    For Each ReceiptItem As BaseReceiptCashListItemType In oCreateReceiptCashListItemsRequest.ReceiptCashListItem
                        iPartyKey = GetAndValidateSpecifiedTableCode(conCashlistItem,
                                                        PMLookupTable.Party, "Party_cnt", "shortname", ReceiptItem.AccountShortCode, oSAMErrorCollection, ReceiptItem.AccountShortCode.ToString())

                        'Get list of OutStanding Amount for the Parties
                        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_get_policies_on_BG_for_receipt")
                            cmd.AddInParameter("@Party_Cnt", SqlDbType.Int).Value = Cast.DefaultIfNull(iPartyKey, 0)
                            dsPolicies = conCashlistItem.ExecuteDataSet(cmd, "Polocies")
                        End Using
                        CreateCashListItem(conCashlistItem, DirectCast(ReceiptItem, BaseCoreCashListItemType), GetCashListForReceipt(conCashlistItem, oCreateReceiptCashListItemsRequest.CashListKey), oSAMErrorCollection)
                        PostCashListItem(conCashlistItem, DirectCast(ReceiptItem, BaseCoreCashListItemType), oCreateReceiptCashListItemsRequest.CashListKey, oSAMErrorCollection)
                        ReDim Preserve oCreateReceiptCashListItemsResponse.CashListItem(iRecordCount)
                        oCreateReceiptCashListItemsResponse.CashListItem(iRecordCount) = New BaseCreateReceiptCashListItemResponseTypeCashListItem
                        oCreateReceiptCashListItemsResponse.CashListItem(iRecordCount).CashListItemKey = oCreateReceiptCashListItemsRequest.ReceiptCashListItem(iRecordCount).CashListItemKey

                        oCreateReceiptCashListItemsResponse.CashListItem(iRecordCount).TransDetailKey = oCreateReceiptCashListItemsRequest.ReceiptCashListItem(iRecordCount).TransDetailKey

                        If ReceiptItem.Policies IsNot Nothing Then
                            For Each PolicyItem As BaseReceiptCashListItemTypePolicies In ReceiptItem.Policies

                                AllocateReceiptsforPolicies(conCashlistItem, PolicyItem, ReceiptItem, oCreateReceiptCashListItemsRequest.CashListKey, shAllcationStatus, dsPolicies)
                                ReDim Preserve oCreateReceiptCashListItemsResponse.CashListItem(iRecordCount)
                                ReDim Preserve oCreateReceiptCashListItemsResponse.CashListItem(iRecordCount).AllocationStatus(iCount)
                                oCreateReceiptCashListItemsResponse.CashListItem(iRecordCount).AllocationStatus(iCount) = CStr(shAllcationStatus)
                                ReDim Preserve oCreateReceiptCashListItemsResponse.CashListItem(iRecordCount).InsuranceFileKey(iCount)
                                oCreateReceiptCashListItemsResponse.CashListItem(iRecordCount).InsuranceFileKey(iCount) = PolicyItem.InsuranceFileKey
                                iCount += 1
                            Next
                        End If
                        iRecordCount += 1
                    Next
                    conCashlistItem.CommitTransaction()
                Catch ex As Exception
                    conCashlistItem.RollbackTransaction()
                End Try
            End If
        End If
        Return oCreateReceiptCashListItemsResponse
    End Function

    Public Sub AllocateReceiptsforPolicies(ByVal conCashlistItem As SiriusConnection,
                                    ByVal oAllocateReceiptsforPoliciesRequest As BaseReceiptCashListItemTypePolicies,
                                    ByVal oCreateReceiptCashListItem As BaseReceiptCashListItemType, ByVal iCashlistKey As Integer,
                                    ByRef r_shAllocationStatus As Short,
                                    ByVal dspolocies As DataSet)

        Dim oResponse As New BaseReceiptCashListItemType
        Dim oErrors As New SAMErrorCollection
        Dim oCoreBusiness As New CoreBusiness
        Dim lReturn As Integer
        Dim iRet As Integer

        Dim obACTCashListPost As New bACTCashListPost.Automated

        If oAllocateReceiptsforPoliciesRequest.InsuranceFileKey <= 0 Then
            oErrors.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                                 SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                                  "InsuranceFileKey")
        End If

        If oAllocateReceiptsforPoliciesRequest.AmountTobeAllocated <= 0.0 Then
            oErrors.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                                                 SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                                                  "AmountTobeAllocated")
        End If

        'Business Validation
        Dim drPolicy As DataRow() = dspolocies.Tables(0).Select("insurance_file_cnt = " & oAllocateReceiptsforPoliciesRequest.InsuranceFileKey)
        If drPolicy Is Nothing Then
            oErrors.AddInvalidData(SAMConstants.SAMBusinessErrors.FailedToAddClaimReceiptItem,
                                                                                    SAMConstants.SAMBusinessErrors.FailedToAddClaimReceiptItem.ToString(),
                                                                                     "Invalid insurance file key")
        End If

        If oAllocateReceiptsforPoliciesRequest.AmountTobeAllocated <= 0.0 Then
            oErrors.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                                                 SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                                                  "DocumentRef")
        End If
        'Following line is being added by rk as part of SAM Interop conversions and as per internal and SSP discussion in place of LOC next to it (now commented somewhere below saying oDatabase = con.PMDAODatabase)
        'conCashlistItem = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
        Dim oDatabase As Object = Nothing '= conCashlistItem.SqlConnection.Database
        'Dim oDatabase As Object = conCashlistItem.PMDAODatabase()
        ' con.BeginTransaction()
        'Mandatory Validation

        If Not conCashlistItem Is Nothing Then
            oDatabase = conCashlistItem.PMDAODatabase
        End If

        Try

            'Data Validation
            GetAndValidateSpecifiedTableCode(conCashlistItem, PMLookupTable.Insurance_file, "insurance_file_cnt", "insurance_file_cnt", oAllocateReceiptsforPoliciesRequest.InsuranceFileKey.ToString(), oErrors, "insurance_file_cnt")
            oErrors.CheckForErrors()

            'oCoreBusiness.UnlockAndGetSAMTS(Con:=con, _
            '                    BranchCode:=oCreateReceiptCashListItemsRequest.BranchCode, _
            '                    Lockname:=CoreBusiness.LockName.InsuranceFileCnt, _
            '                    LockValue:=oAllocateReceiptsforPoliciesRequest.InsuranceFileKey, _
            '                    TStamp:=oAllocateReceiptsforPoliciesRequest.TimeStamp)

            lReturn = CInt(obACTCashListPost.Initialise(
                                  _SiriusUser.Username,
                                  _SiriusUser.Password,
                                  _SiriusUser.UserID,
                                  _SiriusUser.SourceID,
                                  _SiriusUser.LanguageID,
                                  _SiriusUser.CurrencyID,
                                  1,
                                  SiriusUserDefaults.AppName,
                                  vDatabase:=oDatabase))
            iRet = obACTCashListPost.PostAllocatedCashListItemSAM(iCashlistKey,
                                        oCreateReceiptCashListItem.CashListItemKey,
                                        oAllocateReceiptsforPoliciesRequest.InsuranceFileKey,
                                        "",
                                        oAllocateReceiptsforPoliciesRequest.WriteOffReasonKey,
                                        oAllocateReceiptsforPoliciesRequest.WriteOffAmount,
                                        False,
                                        CShort(r_shAllocationStatus),
                                        True,
                                        oAllocateReceiptsforPoliciesRequest.AmountTobeAllocated,
                                        True,
                                        oCreateReceiptCashListItem.AccountKey)

            If (iRet <> PMEReturnCode.PMTrue) Then
                If obACTCashListPost IsNot Nothing Then
                    obACTCashListPost.Dispose()
                    obACTCashListPost = Nothing
                End If
                RaiseComMethodException("obACTCashListPost.PostAllocatedCashListItem", iRet)
            Else
                If (oCreateReceiptCashListItem.TypeCode = "BankGua") Then
                    'Rk this requires change as part of SAM Interop conversion discussed by SSP
                    Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Update_Available_Balance_with_BGKey")
                        cmd.AddInParameter("@Bg_Id", SqlDbType.Int).Value = Cast.DefaultIfNull(oAllocateReceiptsforPoliciesRequest.BGKey, 0)
                        cmd.AddInParameter("@Available_Bal", SqlDbType.Decimal).Value = Cast.DefaultIfNull(oAllocateReceiptsforPoliciesRequest.AmountTobeAllocated, 0)
                        conCashlistItem.ExecuteNonQuery(cmd)
                    End Using
                    Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_update_cashlistitem_for_bg")
                        cmd.AddInParameter("@Bg_Id", SqlDbType.Int).Value = Cast.DefaultIfNull(oAllocateReceiptsforPoliciesRequest.BGKey, 0)
                        cmd.AddInParameter("@cashList_id", SqlDbType.Int).Value = Cast.DefaultIfNull(iCashlistKey, 0)
                        cmd.AddInParameter("@cashListItem_id", SqlDbType.Int).Value = Cast.DefaultIfNull(oCreateReceiptCashListItem.CashListItemKey, 0)
                        cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = Cast.DefaultIfNull(oAllocateReceiptsforPoliciesRequest.InsuranceFileKey, 0)
                        cmd.AddInParameter("@amt_to_be_posted", SqlDbType.Decimal).Value = Cast.DefaultIfNull(oAllocateReceiptsforPoliciesRequest.AmountTobeAllocated, 0)
                        conCashlistItem.ExecuteNonQuery(cmd)
                    End Using
                End If
            End If

        Catch ex As Exception

            Throw
        End Try  'oResponse = AllocateReceiptsforPolicies(con, AllocateReceiptsforPoliciesRequest)

        ' Unlock and return the new timestamp
        'oCoreBusiness.UnlockAndGetSAMTS(Con:=con, _
        '    BranchCode:=oCreateReceiptCashListItemsRequest.BranchCode, _
        '    Lockname:=CoreBusiness.LockName.InsuranceFileCnt, _
        '    LockValue:=oAllocateReceiptsforPoliciesRequest.InsuranceFileKey, _
        '    TStamp:=oAllocateReceiptsforPoliciesRequest.TimeStamp)

        'Return oResponse

    End Sub

    Public Sub AllocatePaymentsforPolicies(ByVal conCashlistItem As SiriusConnection,
                                    ByVal oAllocatePaymentsforPoliciesRequest As BasePaymentCashListItemTypePolicies,
                                    ByVal oCreatePaymentCashListItem As BasePaymentCashListItemType, ByVal iCashlistKey As Integer,
                                    ByRef r_shAllocationStatus As Short)

        Dim oResponse As New BasePaymentCashListItemType
        Dim oErrors As New SAMErrorCollection
        Dim oCoreBusiness As New CoreBusiness
        Dim lReturn As Integer
        Dim iRet As Integer

        Dim obACTCashListPost As New bACTCashListPost.Automated

        If oAllocatePaymentsforPoliciesRequest.InsuranceFileKey <= 0 Then
            oErrors.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                                 SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                                  "InsuranceFileKey")
        End If

        If oAllocatePaymentsforPoliciesRequest.AmountTobeAllocated = 0.0 Then
            oErrors.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                                                 SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                                                  "AmountTobeAllocated")
        End If

        'Following line is being added by rk as part of SAM Interop conversions and as per internal and SSP discussion in place of LOC next to it (now commented somewhere below saying oDatabase = con.PMDAODatabase)
        If conCashlistItem Is Nothing Then
            conCashlistItem = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
        End If
        Dim oDatabase As Object = Nothing '= conCashlistItem.SqlConnection.Database
        If Not conCashlistItem Is Nothing Then
            oDatabase = conCashlistItem.PMDAODatabase
        End If

        'Mandatory Validation
        Try
            GetAndValidateSpecifiedTableCode(conCashlistItem, PMLookupTable.Insurance_file, "insurance_file_cnt", "insurance_file_cnt", oAllocatePaymentsforPoliciesRequest.InsuranceFileKey.ToString(), oErrors, "insurance_file_cnt")
            oErrors.CheckForErrors()

            lReturn = CInt(obACTCashListPost.Initialise(
                                  _SiriusUser.Username,
                                  _SiriusUser.Password,
                                  _SiriusUser.UserID,
                                  _SiriusUser.SourceID,
                                  _SiriusUser.LanguageID,
                                  _SiriusUser.CurrencyID,
                                  1,
                                  SiriusUserDefaults.AppName,
                                  vDatabase:=oDatabase))
            iRet = obACTCashListPost.PostAllocatedCashListItemSAM(iCashlistKey,
                                        oCreatePaymentCashListItem.CashListItemKey,
                                        oAllocatePaymentsforPoliciesRequest.InsuranceFileKey,
                                        "",
                                        oAllocatePaymentsforPoliciesRequest.WriteOffReasonKey,
                                        oAllocatePaymentsforPoliciesRequest.WriteOffAmount,
                                        False,
                                        CShort(r_shAllocationStatus),
                                        True,
                                        oAllocatePaymentsforPoliciesRequest.AmountTobeAllocated,
                                        True,
                                        oCreatePaymentCashListItem.AccountKey)

            If (iRet <> PMEReturnCode.PMTrue) Then
                If obACTCashListPost IsNot Nothing Then
                    obACTCashListPost.Dispose()
                    obACTCashListPost = Nothing
                End If
                RaiseComMethodException("obACTCashListPost.PostAllocatedCashListItem", iRet)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    ''' <summary>
    '''  This method will call another method to do the business
    ''' </summary>
    '''<param name="oCreateReceiptCashListWithItemsRequest">An object of the class BaseCreateReceiptCashListWithItemsRequestType </param>
    Public Overloads Function CreateReceiptCashListWithItems(ByVal oCreateReceiptCashListWithItemsRequest As BaseCreateReceiptCashListWithItemsRequestType) As BaseCreateReceiptCashListWithItemsResponseType
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                             _SiriusUser.Username, _SiriusUser.SourceID,
                                             _SiriusUser.LanguageID,
                                             SiriusUserDefaults.AppName)
            Dim oResponse As BaseCreateReceiptCashListWithItemsResponseType

            oResponse = CreateReceiptCashListWithItems(con, oCreateReceiptCashListWithItemsRequest)

            Return oResponse

        End Using

    End Function

    ''' <summary>
    '''   This method will do the business
    ''' </summary>
    '''<param name="conCashlist">An object of the class SiriusConnection </param>
    '''<param name="oCreateReceiptCashListWithItemsRequest">An object of the class BaseCreateReceiptCashListWithItemsRequestType </param>

    Public Overloads Function CreateReceiptCashListWithItems(ByVal conCashlist As SiriusConnection, ByVal oCreateReceiptCashListWithItemsRequest As BaseCreateReceiptCashListWithItemsRequestType) As BaseCreateReceiptCashListWithItemsResponseType
        Dim oCreateReceiptCashListWithItemsResponse As New BaseCreateReceiptCashListWithItemsResponseType
        Dim oSAMErrorCollection As New SAMErrorCollection
        Dim nTypeOfPackage As enumTypeOfPackage
        Dim oCoreBusiness As New CoreBusiness

        If oCreateReceiptCashListWithItemsRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.CreateReceiptCashListWithItemsRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oCreateReceiptCashListWithItemsResponse = New SAMForInsuranceV2ImplementationTypes.CreateReceiptCashListWithItemsResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If

        '**********************************************************************************************
        ''Structure Validation(Mandatory Checking)
        '**********************************************************************************************
        If (oCreateReceiptCashListWithItemsRequest IsNot Nothing) Then

            oCreateReceiptCashListWithItemsRequest.Validate(CObj(oSAMErrorCollection))

            If (oCreateReceiptCashListWithItemsRequest.ReceiptCashList IsNot Nothing) Then

                oCreateReceiptCashListWithItemsRequest.ReceiptCashList.BranchCode = oCreateReceiptCashListWithItemsRequest.BranchCode

                oCreateReceiptCashListWithItemsRequest.ReceiptCashList.SourceID = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
                   PMLookupTable.Source, oCreateReceiptCashListWithItemsRequest.ReceiptCashList.BranchCode, "BranchCode", oSAMErrorCollection)

                oCreateReceiptCashListWithItemsRequest.ReceiptCashList.Validate(CObj(oSAMErrorCollection))

                oSAMErrorCollection.CheckForErrors()

                '**********************************************************************************************
                'Data Validation
                '**********************************************************************************************
                ValidateBaseCoreCashListType(conCashlist, oCoreBusiness, DirectCast(oCreateReceiptCashListWithItemsRequest.ReceiptCashList, BaseCoreCashListType), oSAMErrorCollection)

                If (oCreateReceiptCashListWithItemsRequest.ReceiptCashList.ReceiptItem IsNot Nothing) Then

                    For Each ReceiptItem As BaseReceiptCashListItemType In oCreateReceiptCashListWithItemsRequest.ReceiptCashList.ReceiptItem
                        'Arul Start
                        ValidateBaseReceiptCashListItemType(oCoreBusiness, conCashlist, DirectCast(oCreateReceiptCashListWithItemsRequest.ReceiptCashList, BaseReceiptCashListType), DirectCast(ReceiptItem, BaseReceiptCashListItemType), oSAMErrorCollection)
                        'Arul End
                    Next
                    oSAMErrorCollection.CheckForErrors()
                End If
                Try
                    'conCashlist.BeginTransaction()
                    CreateCashList(conCashlist, DirectCast(oCreateReceiptCashListWithItemsRequest.ReceiptCashList, BaseCoreCashListType), oSAMErrorCollection)

                    oCreateReceiptCashListWithItemsResponse.CashListKey = oCreateReceiptCashListWithItemsRequest.ReceiptCashList.CashListKey
                    Dim iRecordCount As Integer = 0
                    Dim iCount As Integer = 0
                    Dim shAllcationStatus As Short
                    Dim iPartyKey As Integer
                    Dim iDocumentId As Integer = 0
                    Dim sDocumentRef As String
                    Dim documentCode As String
                    Dim bAutoAllocatePaymentSuccessful As Boolean = True
                    If (oCreateReceiptCashListWithItemsRequest.ReceiptCashList.ReceiptItem IsNot Nothing) Then
                        For Each ReceiptItem As BaseReceiptCashListItemType In oCreateReceiptCashListWithItemsRequest.ReceiptCashList.ReceiptItem
                            Dim dsPolicies As DataSet
                            iPartyKey = GetAndValidateSpecifiedTableCode(conCashlist,
                                             PMLookupTable.Party, "Party_cnt", "shortname", ReceiptItem.AccountShortCode, oSAMErrorCollection, ReceiptItem.AccountShortCode.ToString())

                            If ReceiptItem.Amount <> 0.0 Then
                                If ReceiptItem.AllocationDetails IsNot Nothing AndAlso ReceiptItem.AllocationDetails.AutoAllocate Then
                                    ProcessCashListItem(conCashlist, oCoreBusiness,
                                           DirectCast(oCreateReceiptCashListWithItemsRequest.ReceiptCashList, BaseCoreCashListType),
                                           DirectCast(ReceiptItem, BaseCoreCashListItemType),
                                           oSAMErrorCollection,
                                           bAutoAllocatePaymentSuccessful:=bAutoAllocatePaymentSuccessful)
                                Else

                                    CreateCashListItem(conCashlist, DirectCast(ReceiptItem, BaseCoreCashListItemType), DirectCast(oCreateReceiptCashListWithItemsRequest.ReceiptCashList, BaseCoreCashListType), oSAMErrorCollection)
                                    PostCashListItem(conCashlist, DirectCast(ReceiptItem, BaseCoreCashListItemType), oCreateReceiptCashListWithItemsResponse.CashListKey, oSAMErrorCollection)
                                End If
                                If ReceiptItem.IsProduceDocument Then
                                    iDocumentId = GetAndValidateSpecifiedTableCode(conCashlist,
                                         "transdetail", "document_id", "account_id", ReceiptItem.AccountKey.ToString(), oSAMErrorCollection, "")

                                    If iDocumentId <> 0 Then
                                        sDocumentRef = GetAndValidateDescriptionById(conCashlist, "document", "document_ref", "document_id", iDocumentId.ToString())

                                        GenerateCashChequeReceiptAndPaymentDocument(con:=conCashlist, v_sBranchCode:=oCreateReceiptCashListWithItemsRequest.BranchCode,
                                                                                v_nSourceId:=oCreateReceiptCashListWithItemsRequest.ReceiptCashList.SourceID, v_nPartyKey:=iPartyKey, v_sType:="R", v_sDocumentRef:=sDocumentRef, v_sDocumentCode:=documentCode)
                                    End If
                                End If
                            End If


                            'Get list of OutStanding Amount for the Parties
                            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_get_policies_on_BG_for_receipt")
                                cmd.AddInParameter("@Party_Cnt", SqlDbType.Int).Value = Cast.DefaultIfNull(iPartyKey, 0)
                                dsPolicies = conCashlist.ExecuteDataSet(cmd, "Policies")
                            End Using

                            If ReceiptItem.Policies IsNot Nothing Then

                                For Each PolicyItem As BaseReceiptCashListItemTypePolicies In ReceiptItem.Policies
                                    If PolicyItem.AmountTobeAllocated <> 0 Then
                                        AllocateReceiptsforPolicies(conCashlist, PolicyItem, ReceiptItem, oCreateReceiptCashListWithItemsRequest.ReceiptCashList.CashListKey, shAllcationStatus, dsPolicies)
                                    End If
                                Next

                            End If

                            ReDim Preserve oCreateReceiptCashListWithItemsResponse.CashListItem(iRecordCount)
                            oCreateReceiptCashListWithItemsResponse.CashListItem(iRecordCount) = New BaseCreateReceiptCashListWithItemsResponseTypeCashListItem
                            oCreateReceiptCashListWithItemsResponse.CashListItem(iRecordCount).AccountShortCode = oCreateReceiptCashListWithItemsRequest.ReceiptCashList.ReceiptItem(iRecordCount).AccountShortCode
                            oCreateReceiptCashListWithItemsResponse.CashListItem(iRecordCount).CashListItemKey = oCreateReceiptCashListWithItemsRequest.ReceiptCashList.ReceiptItem(iRecordCount).CashListItemKey
                            oCreateReceiptCashListWithItemsResponse.CashListItem(iRecordCount).TransDetailKey = oCreateReceiptCashListWithItemsRequest.ReceiptCashList.ReceiptItem(iRecordCount).TransDetailKey
                            oCreateReceiptCashListWithItemsResponse.CashListItem(iRecordCount).DocumentRef = sDocumentRef
                            oCreateReceiptCashListWithItemsResponse.CashListItem(iRecordCount).DocumentCode = documentCode
                            oCreateReceiptCashListWithItemsResponse.CashListItem(iRecordCount).AutoAllocatePaymentSuccessful = bAutoAllocatePaymentSuccessful
                            iRecordCount += 1
                        Next
                    End If

                    ' RichardT - Check the transactions and if there are none open then don't comiit it.  This is done because 
                    '            further down the chain bACTInstalments and bACTAllocationManual closes the open transaction causing a failure. 
                    'If conCashlist.TransactionCount > 0 Then
                    'conCashlist.CommitTransaction()

                Catch ex As Exception
                    ' RichardT - Check the transactions and if there are none open then don't comiit it.  This is done because 
                    '            further down the chain bACTInstalments and bACTAllocationManual closes the open transaction causing a failure. 
                    'If conCashlist.TransactionCount > 0 Then
                    'conCashlist.RollbackTransaction()
                    Throw

                End Try
            End If
        Else
            Return Nothing
        End If
        Return oCreateReceiptCashListWithItemsResponse
    End Function

    ''' <summary>  
    '''  This method is used to find accounts in claim payment
    '''<param name="FindAccountsRequest" type="BaseFindAccountsRequestType"></param>   
    '''<returns>BaseFindAccountsResponseType</returns>
    '''<remarks></remarks>
    Public Overloads Function FindAccounts(ByVal oFindAccountsRequest As BaseFindAccountsRequestType) As BaseFindAccountsResponseType
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)
            Dim oResponse As BaseFindAccountsResponseType

            oResponse = FindAccounts(con, oFindAccountsRequest)

            Return oResponse

        End Using

    End Function

    ''' <summary>  
    '''  This method is used to find accounts in claim payment
    '''<param name="FindAccountsRequest" type="BaseFindAccountsRequestType"></param>   
    '''<returns>BaseFindAccountsResponseType</returns>
    '''<remarks></remarks>
    Public Overloads Function FindAccounts(ByVal con As SiriusConnection, ByVal oFindAccountsRequest As BaseFindAccountsRequestType) As BaseFindAccountsResponseType

        Dim oFindAccountsResponse As New BaseFindAccountsResponseType
        Dim oCoreBusiness As New CoreBusiness
        Dim oErrors As New SAMErrorCollection
        Dim ibranchId As Integer
        Dim iAccountTypeId As Integer
        Dim iLedgerId As Integer

        Dim nTypeOfPackage As enumTypeOfPackage

        If oFindAccountsRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.FindAccountsRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage
            oFindAccountsResponse = New SAMForInsuranceV2ImplementationTypes.FindAccountsResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If

        'Manditory validation
        oFindAccountsRequest.Validate(CObj(oErrors))
        oErrors.CheckForErrors()
        ' Lookup Codes to ensure they are valid, i.e.
        Try
            If String.IsNullOrEmpty(oFindAccountsRequest.BranchCode) = False Then
                ibranchId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.Source, oFindAccountsRequest.BranchCode, "BranchCode")
            End If
        Catch ex As Exception
            oErrors.AddInvalidData(SAMInvalidData.InvalidLookupListValue,
                                        SAMInvalidData.InvalidLookupListValue.ToString,
                                        "BranchCode",
                                        oFindAccountsRequest.BranchCode)
        End Try

        Try
            If String.IsNullOrEmpty(oFindAccountsRequest.AccountTypeCode) = False Then

                iAccountTypeId = oCoreBusiness.GetAndValidateListItemFromCode(CType(STSListType.PMLookup, Core.STSListType), "accounttype", oFindAccountsRequest.AccountTypeCode, "accounttype_id", oErrors)
            End If

        Catch ex As Exception
            oErrors.AddInvalidData(SAMInvalidData.InvalidLookupListValue,
                                        SAMInvalidData.InvalidLookupListValue.ToString,
                                        "AccountTypeCode",
                                        oFindAccountsRequest.AccountTypeCode)
        End Try
        Try
            If String.IsNullOrEmpty(oFindAccountsRequest.LedgerCode) = False Then
                iLedgerId = GetAndValidateSpecifiedTableCode(con,
                                                                    PMLookupTable.ledger,
                                                                    "ledger_id",
                                                                    "ledger_short_name",
                                                                    oFindAccountsRequest.LedgerCode,
                                                                  oErrors, "LedgerCode")
            End If
        Catch ex As Exception
            oErrors.AddInvalidData(SAMInvalidData.InvalidLookupListValue,
                                        SAMInvalidData.InvalidLookupListValue.ToString,
                                        "LedgerCode",
                                        oFindAccountsRequest.LedgerCode)
        End Try

        ' exit if there are any missing parameters
        oErrors.CheckForErrors()
        Dim sPaymentLedgers As String = Nothing
        Dim sProductOption As String
        Dim iNoOfRecords As Integer = 500
        Dim vResultArray As Object(,) = Nothing
        Dim vFullKey As Object = "Full Key"
        Dim oBOFindAccount As New bACTFindAccount.Business
        Dim iComReturnValue As Integer
        Dim dsFindAccounts As New DataSet
        Dim docFindAccount As New System.Xml.XmlDocument
        Dim lReturn As Integer = 0

        If oFindAccountsRequest.MaxRowsToFetchSpecified Then
            iNoOfRecords = oFindAccountsRequest.MaxRowsToFetch
        End If

        Try
            'This portion will call the com method to initialize
            lReturn = CInt(oBOFindAccount.Initialise(
                             _SiriusUser.Username,
                             _SiriusUser.Password,
                             _SiriusUser.UserID,
                             _SiriusUser.SourceID,
                             _SiriusUser.LanguageID,
                             _SiriusUser.CurrencyID,
                             1,
                             SiriusUserDefaults.AppName))

            If (lReturn <> PMEReturnCode.PMTrue) Then

                ' if the account processing fails then throw a business rule error
                oErrors.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed,
                                                    SAMBusinessErrors.COMComponentMethodFailed.ToString,
                                                    "bACTFindAccount.Business.Initialise")
                oErrors.CheckForErrors()

            End If
            If oFindAccountsRequest.IncludeInsurerAgents Then
                iComReturnValue = oBOFindAccount.GetPaymentLedgers(sPaymentLedgers)
                sProductOption = oCoreBusiness.GetProductOption(SIRHiddenOptions.SIROPTEnhancedOrionSecurity, ibranchId)
                If sProductOption = "1" Then
                    iComReturnValue = oBOFindAccount.SelectAccountQueryFiltered(lNumberOfRecords:=iNoOfRecords, vresultArray:=vResultArray, iCompanyID:=Convert.ToInt16(ibranchId), vFullKey:=vFullKey, vLedgerID:=IIf(iLedgerId > 0, iLedgerId, -1), vAccountName:=oFindAccountsRequest.AccountName, vAccountType:=IIf(iAccountTypeId > 0, iAccountTypeId, -1), vShortCode:=oFindAccountsRequest.ShortCode, vInsuranceRef:=oFindAccountsRequest.InsuranceRef, vOperatorID:=IIf(oFindAccountsRequest.OperatorKeySpecified, oFindAccountsRequest.OperatorKey, Nothing), vPurchaseOrderNo:=oFindAccountsRequest.PurchaseOrderNo, vPurchaseInvoiceNo:=oFindAccountsRequest.PurchaseInvoiceNo, vSpare:=oFindAccountsRequest.Spare, vShowDeleted:=IIf(oFindAccountsRequest.ShowDeleted, 1, 0), v_bOnlyUpdatable:=Convert.ToBoolean(IIf(oFindAccountsRequest.OnlyUpdatableAccounts, 1, 0)), vPaymentLedgerIDs:=sPaymentLedgers)
                Else
                    iComReturnValue = oBOFindAccount.SelectAccountQuery(lNumberOfRecords:=iNoOfRecords, vresultArray:=vResultArray, iCompanyID:=Convert.ToInt16(ibranchId), vFullKey:=vFullKey, vLedgerID:=IIf(iLedgerId > 0, iLedgerId, -1), vAccountName:=oFindAccountsRequest.AccountName, vAccountType:=IIf(iAccountTypeId > 0, iAccountTypeId, -1), vShortCode:=oFindAccountsRequest.ShortCode, vInsuranceRef:=oFindAccountsRequest.InsuranceRef, vOperatorID:=IIf(oFindAccountsRequest.OperatorKeySpecified, oFindAccountsRequest.OperatorKey, Nothing), vPurchaseOrderNo:=oFindAccountsRequest.PurchaseOrderNo, vPurchaseInvoiceNo:=oFindAccountsRequest.PurchaseInvoiceNo, vSpare:=oFindAccountsRequest.Spare, vShowDeleted:=IIf(oFindAccountsRequest.ShowDeleted, 1, 0), vPaymentLedgerIDs:=sPaymentLedgers, vShowBalance:=IIf(oFindAccountsRequest.ShowBalance, 1, 0))
                End If
            ElseIf oFindAccountsRequest.ExcludeInsurerAgents Then
                iComReturnValue = oBOFindAccount.GetPaymentLedgers(sPaymentLedgers)
                sProductOption = oCoreBusiness.GetProductOption(SIRHiddenOptions.SIROPTEnhancedOrionSecurity, ibranchId)
                If sProductOption = "1" Then
                    iComReturnValue = oBOFindAccount.SelectAccountQueryFiltered(lNumberOfRecords:=iNoOfRecords, vresultArray:=vResultArray, iCompanyID:=Convert.ToInt16(ibranchId), vFullKey:=vFullKey, vLedgerID:=IIf(iLedgerId > 0, iLedgerId, -1), vAccountName:=oFindAccountsRequest.AccountName, vAccountType:=IIf(iAccountTypeId > 0, iAccountTypeId, -1), vShortCode:=oFindAccountsRequest.ShortCode, vInsuranceRef:=oFindAccountsRequest.InsuranceRef, vOperatorID:=IIf(oFindAccountsRequest.OperatorKeySpecified, oFindAccountsRequest.OperatorKey, Nothing), vPurchaseOrderNo:=oFindAccountsRequest.PurchaseOrderNo, vPurchaseInvoiceNo:=oFindAccountsRequest.PurchaseInvoiceNo, vSpare:=oFindAccountsRequest.Spare, vShowDeleted:=IIf(oFindAccountsRequest.ShowDeleted, 1, 0), v_bOnlyUpdatable:=Convert.ToBoolean(IIf(oFindAccountsRequest.OnlyUpdatableAccounts, 1, 0)), vExcludeLedgerIDs:=sPaymentLedgers)
                Else
                    iComReturnValue = oBOFindAccount.SelectAccountQuery(lNumberOfRecords:=iNoOfRecords, vresultArray:=vResultArray, iCompanyID:=Convert.ToInt16(ibranchId), vFullKey:=vFullKey, vLedgerID:=IIf(iLedgerId > 0, iLedgerId, -1), vAccountName:=oFindAccountsRequest.AccountName, vAccountType:=IIf(iAccountTypeId > 0, iAccountTypeId, -1), vShortCode:=oFindAccountsRequest.ShortCode, vInsuranceRef:=oFindAccountsRequest.InsuranceRef, vOperatorID:=IIf(oFindAccountsRequest.OperatorKeySpecified, oFindAccountsRequest.OperatorKey, Nothing), vPurchaseOrderNo:=oFindAccountsRequest.PurchaseOrderNo, vPurchaseInvoiceNo:=oFindAccountsRequest.PurchaseInvoiceNo, vSpare:=oFindAccountsRequest.Spare, vShowDeleted:=IIf(oFindAccountsRequest.ShowDeleted, 1, 0), vExcludeLedgerIDs:=sPaymentLedgers, vShowBalance:=IIf(oFindAccountsRequest.ShowBalance, 1, 0))
                End If
            Else
                sProductOption = oCoreBusiness.GetProductOption(SIRHiddenOptions.SIROPTEnhancedOrionSecurity, ibranchId)
                If sProductOption = "1" Then
                    iComReturnValue = oBOFindAccount.SelectAccountQueryFiltered(lNumberOfRecords:=iNoOfRecords, vresultArray:=vResultArray, iCompanyID:=Convert.ToInt16(ibranchId), vFullKey:=vFullKey, vLedgerID:=IIf(iLedgerId > 0, iLedgerId, -1), vAccountName:=oFindAccountsRequest.AccountName, vAccountType:=IIf(iAccountTypeId > 0, iAccountTypeId, -1), vShortCode:=oFindAccountsRequest.ShortCode, vInsuranceRef:=oFindAccountsRequest.InsuranceRef, vOperatorID:=IIf(oFindAccountsRequest.OperatorKeySpecified, oFindAccountsRequest.OperatorKey, Nothing), vPurchaseOrderNo:=oFindAccountsRequest.PurchaseOrderNo, vPurchaseInvoiceNo:=oFindAccountsRequest.PurchaseInvoiceNo, vSpare:=oFindAccountsRequest.Spare, vShowDeleted:=IIf(oFindAccountsRequest.ShowDeleted, 1, 0), v_bOnlyUpdatable:=Convert.ToBoolean(IIf(oFindAccountsRequest.OnlyUpdatableAccounts, 1, 0)))
                Else
                    iComReturnValue = oBOFindAccount.SelectAccountQuery(lNumberOfRecords:=iNoOfRecords, vresultArray:=vResultArray, iCompanyID:=Convert.ToInt16(ibranchId), vFullKey:=vFullKey, vLedgerID:=IIf(iLedgerId > 0, iLedgerId, -1), vAccountName:=oFindAccountsRequest.AccountName, vAccountType:=IIf(iAccountTypeId > 0, iAccountTypeId, -1), vShortCode:=oFindAccountsRequest.ShortCode, vInsuranceRef:=oFindAccountsRequest.InsuranceRef, vOperatorID:=IIf(oFindAccountsRequest.OperatorKeySpecified, oFindAccountsRequest.OperatorKey, Nothing), vPurchaseOrderNo:=oFindAccountsRequest.PurchaseOrderNo, vPurchaseInvoiceNo:=oFindAccountsRequest.PurchaseInvoiceNo, vSpare:=oFindAccountsRequest.Spare, vShowDeleted:=IIf(oFindAccountsRequest.ShowDeleted, 1, 0), vShowBalance:=IIf(oFindAccountsRequest.ShowBalance, 1, 0), vBrokerCnt:=oFindAccountsRequest.BrokerCnt)
                End If
            End If

            If iComReturnValue <> PMEReturnCode.PMTrue AndAlso iComReturnValue <> PMReturnCode.PMNotFound Then
                RaiseComMethodException("bACTFindAccount.Business.FindAccounts", iComReturnValue)
            End If

            dsFindAccounts = Utilities.ArrayToDataSet(vResultArray, "BaseFindAccountsResponseTypeAccounts")
            If dsFindAccounts IsNot Nothing Then
                If dsFindAccounts.Tables(0).Rows.Count > 0 Then
                    dsFindAccounts.DataSetName = "BaseFindAccountsResponseTypeAccounts"
                    dsFindAccounts.Tables(0).TableName = "Row"
                    dsFindAccounts.Tables(0).Columns(0).ColumnName = "FullKey"
                    dsFindAccounts.Tables(0).Columns(1).ColumnName = "ShortCode"
                    dsFindAccounts.Tables(0).Columns(2).ColumnName = "AccountName"
                    dsFindAccounts.Tables(0).Columns(3).ColumnName = "LedgerKey"
                    dsFindAccounts.Tables(0).Columns(4).ColumnName = "AccountTypeKey"
                    dsFindAccounts.Tables(0).Columns(5).ColumnName = "AccountKey"
                    dsFindAccounts.Tables(0).Columns(6).ColumnName = "PartyKey"
                    dsFindAccounts.Tables(0).Columns(7).ColumnName = "NominalAccountKey"
                    dsFindAccounts.Tables(0).Columns(8).ColumnName = "AccountStatus"
                    dsFindAccounts.Tables(0).Columns(9).ColumnName = "AccountStatusKey"
                    dsFindAccounts.Tables(0).Columns(10).ColumnName = "CompanyKey"
                    dsFindAccounts.Tables(0).Columns(11).ColumnName = "AccountBalance"
                    dsFindAccounts.Tables(0).Columns(12).ColumnName = "ContactName"
                    dsFindAccounts.Tables(0).Columns(13).ColumnName = "AddressLine1"
                    dsFindAccounts.Tables(0).Columns(14).ColumnName = "PersonalClientForename"
                    dsFindAccounts.Tables(0).Columns(15).ColumnName = "CurrencyId"
                    dsFindAccounts.Tables(0).Columns(16).ColumnName = "CurrencyCode"
                    dsFindAccounts.Tables(0).Columns(17).ColumnName = "SourceId"
                    dsFindAccounts.Tables(0).Columns(18).ColumnName = "SourceCode"
                    dsFindAccounts.Tables(0).Columns(19).ColumnName = "IsGrossAgent"
                    dsFindAccounts.Tables(0).Columns.Add("LedgerCode", GetType(System.String))
                    dsFindAccounts.Tables(0).Columns.Add("AccountTypeCode", GetType(System.String))
                    Dim oRow As DataRow
                    For Each oRow In dsFindAccounts.Tables(0).Rows
                        oRow.Item(20) = GetAndValidateDescriptionById(con, "Ledger", "ledger_short_name", "ledger_id", oRow.Item(3).ToString)
                        oRow.Item(21) = GetAndValidateDescriptionById(con, "AccountType", "description", "AccountType_id", oRow.Item(4).ToString)
                    Next
                    If oFindAccountsRequest.ShowBalance = False Then
                        dsFindAccounts.Tables(0).Columns.RemoveAt(11)
                        dsFindAccounts.Tables(0).Columns.Add("AccountBalance", GetType(System.Double))
                    End If
                    If oFindAccountsRequest.WCFSecurityToken = "" Then
                        'These are the column index needs to remove from dataset                    
                        Dim xmlDoc As New System.Xml.XmlDocument
                        xmlDoc.LoadXml(dsFindAccounts.GetXml)
                        oFindAccountsResponse.ResultDataset = xmlDoc.DocumentElement()
                    End If
                    oFindAccountsResponse.ResultData = dsFindAccounts
                Else
                    oFindAccountsResponse.ResultDataset = Nothing
                    oFindAccountsResponse.ResultData = Nothing
                End If
            Else
                oFindAccountsResponse.ResultDataset = Nothing
                oFindAccountsResponse.ResultData = Nothing
            End If
        Catch ex As Exception

            Throw New Exception("Failed to call bACTFindAccount.Business.FindAccounts", ex)
        Finally
            If oBOFindAccount IsNot Nothing Then
                oBOFindAccount.Dispose()
                oBOFindAccount = Nothing
            End If
        End Try

        Return oFindAccountsResponse

    End Function

    ''' <summary>  
    '''  This method is used to get Payment Cash List Items
    '''<param name="oGetPaymentCashListDetailsRequest" type="BaseGetPaymentCashListDetailsRequestType"></param>   
    '''<returns>BaseGetPaymentCashListDetailsResponseType</returns>
    '''<remarks></remarks>
    Public Overloads Function GetPaymentCashListDetails(ByVal oGetPaymentCashListDetailsRequest As BaseGetPaymentCashListDetailsRequestType) As BaseGetPaymentCashListDetailsResponseType
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)
            Dim oResponse As BaseGetPaymentCashListDetailsResponseType

            oResponse = GetPaymentCashListDetails(con, oGetPaymentCashListDetailsRequest)

            Return oResponse

        End Using

    End Function

    ''' <summary>
    '''This method is used to get Payment Cash List Items
    '''</summary>
    '''<param name="oGetPaymentCashListDetailsRequest" type="BaseGetPaymentCashListDetailsRequestType"></param>
    '''<param name="con" type="SiriusConnection"></param>
    '''<remarks></remarks>
    Public Overloads Function GetPaymentCashListDetails(ByVal con As SiriusConnection, ByVal oGetPaymentCashListDetailsRequest As BaseGetPaymentCashListDetailsRequestType) As BaseGetPaymentCashListDetailsResponseType

        Dim oGetPaymentCashListDetailsResponse As New BaseGetPaymentCashListDetailsResponseType
        Dim oCoreBusiness As New CoreBusiness
        Dim oErrors As New SAMErrorCollection

        Dim ibranchId As Integer
        Dim nTypeOfPackage As enumTypeOfPackage

        If oGetPaymentCashListDetailsRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.GetPaymentCashListDetailsRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oGetPaymentCashListDetailsResponse = New SAMForInsuranceV2ImplementationTypes.GetPaymentCashListDetailsResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If

        'Validate the mandatory parameters
        oGetPaymentCashListDetailsRequest.Validate(CObj(oErrors))
        oErrors.CheckForErrors()

        ' Lookup Codes to ensure they are valid – To be done for all codes in the request, e.g.
        Try
            ibranchId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.Source, oGetPaymentCashListDetailsRequest.BranchCode, "BranchCode")
        Catch ex As Exception
            oErrors.AddInvalidData(SAMInvalidData.InvalidLookupListValue,
                                        SAMInvalidData.InvalidLookupListValue.ToString,
                                        "BranchCode",
                                        oGetPaymentCashListDetailsRequest.BranchCode)
        End Try

        oErrors.CheckForErrors()

        Dim oBasePaymentCashListType As New BasePaymentCashListType

        oBasePaymentCashListType = CType(GetCashListforPayment(con, oGetPaymentCashListDetailsRequest.CashListKey), BaseImplementationTypes.BasePaymentCashListType)

        oGetPaymentCashListDetailsResponse.PaymentCashList = New BasePaymentCashListType

        oGetPaymentCashListDetailsResponse.PaymentCashList.TypeCode = oBasePaymentCashListType.TypeCode
        oGetPaymentCashListDetailsResponse.PaymentCashList.ListDate = oBasePaymentCashListType.ListDate
        oGetPaymentCashListDetailsResponse.PaymentCashList.BankAccountCode = oBasePaymentCashListType.BankAccountCode
        oGetPaymentCashListDetailsResponse.PaymentCashList.CurrencyCode = oBasePaymentCashListType.CurrencyCode
        oGetPaymentCashListDetailsResponse.PaymentCashList.Reference = oBasePaymentCashListType.Reference
        oGetPaymentCashListDetailsResponse.PaymentCashList.StatusCode = oBasePaymentCashListType.StatusCode

        Return oGetPaymentCashListDetailsResponse
    End Function

    ''' <summary>  
    '''  This method is used to get Payment Cash List Items details
    '''<param name="oGetPaymentCashListItemDetailsRequest" type="BaseGetPaymentCashListItemDetailsRequestType"></param>   
    '''<returns>BaseGetPaymentCashListDetailsResponseType</returns>
    '''<remarks></remarks>

    Public Overloads Function GetPaymentCashListItemDetails(ByVal oGetPaymentCashListItemDetailsRequest As BaseGetPaymentCashListItemDetailsRequestType) As BaseGetPaymentCashListItemDetailsResponseType
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)
            Dim oResponse As BaseGetPaymentCashListItemDetailsResponseType

            oResponse = GetPaymentCashListItemDetails(con, oGetPaymentCashListItemDetailsRequest)

            Return oResponse

        End Using

    End Function

    ''' <summary>
    '''This method is used to get Payment Cash List Items details
    '''</summary>
    '''<param name="oGetPaymentCashListItemDetailsRequest" type="BaseGetPaymentCashListItemDetailsRequestType"></param>
    '''<param name="con" type="SiriusConnection"></param>
    '''<remarks></remarks>
    Public Overloads Function GetPaymentCashListItemDetails(ByVal con As SiriusConnection, ByVal oGetPaymentCashListItemDetailsRequest As BaseGetPaymentCashListItemDetailsRequestType) As BaseGetPaymentCashListItemDetailsResponseType

        Dim oGetPaymentCashListItemDetailsResponse As New BaseGetPaymentCashListItemDetailsResponseType
        Dim oCoreBusiness As New CoreBusiness
        Dim oErrors As New SAMErrorCollection
        Dim dsPaymentCashListItem As New DataSet

        Dim ibranchId As Integer
        Dim nTypeOfPackage As enumTypeOfPackage

        If oGetPaymentCashListItemDetailsRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.GetPaymentCashListItemDetailsRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oGetPaymentCashListItemDetailsResponse = New SAMForInsuranceV2ImplementationTypes.GetPaymentCashListItemDetailsResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If

        'Validate the mandatory parameters
        oGetPaymentCashListItemDetailsRequest.Validate(CObj(oErrors))
        oErrors.CheckForErrors()

        ' Lookup Codes to ensure they are valid – To be done for all codes in the request, e.g.
        Try
            ibranchId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.Source, oGetPaymentCashListItemDetailsRequest.BranchCode, "BranchCode")
        Catch ex As Exception
            oErrors.AddInvalidData(SAMInvalidData.InvalidLookupListValue,
                                        SAMInvalidData.InvalidLookupListValue.ToString,
                                        "BranchCode",
                                        oGetPaymentCashListItemDetailsRequest.BranchCode)
        End Try

        ' exit if there are any missing parameters
        oErrors.CheckForErrors()

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_ACT_Select_CashListItem")
            cmd.AddInParameter("@cashlistitem_id", SqlDbType.Int).Value = oGetPaymentCashListItemDetailsRequest.CashListItemKey

            dsPaymentCashListItem = con.ExecuteDataSet(cmd, "CashListPayment")

        End Using

        'Filling up the Response object
        If dsPaymentCashListItem IsNot Nothing AndAlso dsPaymentCashListItem.Tables.Count > 0 AndAlso dsPaymentCashListItem.Tables(0).Rows.Count > 0 Then
            oGetPaymentCashListItemDetailsResponse.CashListPayment = New BasePaymentCashListItemType
            With oGetPaymentCashListItemDetailsResponse.CashListPayment

                .IsProduceDocument = Cast.ToBoolean(dsPaymentCashListItem.Tables(0).Rows(0).Item("letter"), False)

                .MediaTypeKey = Cast.ToInt32(dsPaymentCashListItem.Tables(0).Rows(0).Item("mediatype_id"), 0)
                .MediaTypeCode = GetAndValidateDescriptionById(con, "mediatype", "code", "mediatype_id", .MediaTypeKey.ToString())
                .TransactionDate = Cast.ToDateTime(dsPaymentCashListItem.Tables(0).Rows(0).Item("transaction_date"), Date.MinValue)
                .AccountKey = Cast.ToInt32(dsPaymentCashListItem.Tables(0).Rows(0).Item("account_id"), 0)
                .AccountShortCode = GetAndValidateDescriptionById(con, "Account", "short_code", "account_id", .AccountKey.ToString())
                .Amount = Cast.ToDecimal(dsPaymentCashListItem.Tables(0).Rows(0).Item("amount"), 0)
                .AllocationStatusKey = Cast.ToInt32(dsPaymentCashListItem.Tables(0).Rows(0).Item("allocationstatus_id"), 0)
                .AllocationStatusCode = GetAndValidateDescriptionById(con, "AllocationStatus", "code", "allocationstatus_id", .AllocationStatusKey.ToString())
                .MediaReference = Cast.ToString(dsPaymentCashListItem.Tables(0).Rows(0).Item("media_ref"), String.Empty)
                .OurReference = Cast.ToString(dsPaymentCashListItem.Tables(0).Rows(0).Item("our_ref"), String.Empty)
                .TheirReference = Cast.ToString(dsPaymentCashListItem.Tables(0).Rows(0).Item("their_ref"), String.Empty)
                .ContactName = Cast.ToString(dsPaymentCashListItem.Tables(0).Rows(0).Item("contact_name"), String.Empty)
                .TypeKey = Cast.ToInt32(dsPaymentCashListItem.Tables(0).Rows(0).Item("cashlistitem_payment_type_id"), 0)
                .TypeCode = GetAndValidateDescriptionById(con, "CashListItem_Payment_Type", "code", "cashlistitem_payment_type_id", .TypeKey.ToString())
                .StatusKey = Cast.ToInt32(dsPaymentCashListItem.Tables(0).Rows(0).Item("cashlistitem_payment_status_id"), 0)
                .StatusCode = GetAndValidateDescriptionById(con, "CashListItem_Payment_status", "code", "cashlistitem_payment_status_id", .StatusKey.ToString())
                .FurtherDetails = Cast.ToString(dsPaymentCashListItem.Tables(0).Rows(0).Item("Receipt_details"), String.Empty)
                .UserId = Cast.ToInt32(dsPaymentCashListItem.Tables(0).Rows(0).Item("pmuser_id"), 0)
                .UserName = Cast.ToString(dsPaymentCashListItem.Tables(0).Rows(0).Item("username"), String.Empty)
                If Cast.ToInt32(dsPaymentCashListItem.Tables(0).Rows(0).Item("tax_band_id"), 0) > 0 Then
                    .TaxBandCode = GetAndValidateDescriptionById(con, "tax_band", "code", "tax_band_id", Cast.ToString(dsPaymentCashListItem.Tables(0).Rows(0).Item("tax_band_id"), String.Empty))
                End If
            End With

            oGetPaymentCashListItemDetailsResponse.CashListPayment.Bank = New BaseBankPaymentType
            With oGetPaymentCashListItemDetailsResponse.CashListPayment.Bank
                .AccountCode = Cast.ToString(dsPaymentCashListItem.Tables(0).Rows(0).Item("payment_account_code"), String.Empty)
                .BranchCode = Cast.ToString(dsPaymentCashListItem.Tables(0).Rows(0).Item("payment_branch_code"), String.Empty)
                .ExpiryDate = Cast.ToDateTime(dsPaymentCashListItem.Tables(0).Rows(0).Item("payment_expiry_date"), Date.MinValue)
                .Reference1 = Cast.ToString(dsPaymentCashListItem.Tables(0).Rows(0).Item("payment_reference1"), String.Empty)
                .Reference2 = Cast.ToString(dsPaymentCashListItem.Tables(0).Rows(0).Item("payment_reference2"), String.Empty)
                .PayeeName = Cast.ToString(dsPaymentCashListItem.Tables(0).Rows(0).Item("payment_name"), String.Empty)
                .BIC = Cast.ToString(dsPaymentCashListItem.Tables(0).Rows(0).Item("business_identifier_code"), String.Empty)
                .IBAN = Cast.ToString(dsPaymentCashListItem.Tables(0).Rows(0).Item("international_bank_account_number"), String.Empty)
            End With

            oGetPaymentCashListItemDetailsResponse.CashListPayment.ContactAddress = New BaseSimpleAddressType

            With oGetPaymentCashListItemDetailsResponse.CashListPayment.ContactAddress
                .AddressLine1 = Cast.ToString(dsPaymentCashListItem.Tables(0).Rows(0).Item("address1"), String.Empty)
                .AddressLine2 = Cast.ToString(dsPaymentCashListItem.Tables(0).Rows(0).Item("address2"), String.Empty)
                .AddressLine3 = Cast.ToString(dsPaymentCashListItem.Tables(0).Rows(0).Item("address3"), String.Empty)
                .AddressLine4 = Cast.ToString(dsPaymentCashListItem.Tables(0).Rows(0).Item("address4"), String.Empty)
                .CountryKey = Cast.ToInt16(dsPaymentCashListItem.Tables(0).Rows(0).Item("address_country"), 0)
                .CountryCode = GetAndValidateDescriptionById(con, "country", "code", "country_id", .CountryKey.ToString())
                .PostCode = Cast.ToString(dsPaymentCashListItem.Tables(0).Rows(0).Item("postal_code"), String.Empty)
            End With

            oGetPaymentCashListItemDetailsResponse.CashListPayment.CreditCard = New BaseCreditCardType
            With oGetPaymentCashListItemDetailsResponse.CashListPayment.CreditCard
                .Number = Cast.ToString(dsPaymentCashListItem.Tables(0).Rows(0).Item("cc_number"), String.Empty)
                .ExpiryDate = Cast.ToString(dsPaymentCashListItem.Tables(0).Rows(0).Item("cc_expiry_date"), String.Empty)
                .StartDate = Cast.ToString(dsPaymentCashListItem.Tables(0).Rows(0).Item("cc_start_date"), String.Empty)
                .AuthCode = Cast.ToString(dsPaymentCashListItem.Tables(0).Rows(0).Item("cc_auth_code"), String.Empty)
                .ManualAuthCode = Cast.ToString(dsPaymentCashListItem.Tables(0).Rows(0).Item("cc_manual_auth_code"), String.Empty)
                .NameOnCreditCard = Cast.ToString(dsPaymentCashListItem.Tables(0).Rows(0).Item("cc_name"), String.Empty)
                .Issue = Cast.ToString(dsPaymentCashListItem.Tables(0).Rows(0).Item("cc_issue"), String.Empty)
                .Pin = Cast.ToString(dsPaymentCashListItem.Tables(0).Rows(0).Item("cc_pin"), String.Empty)
                .TransactionCode = Cast.ToString(dsPaymentCashListItem.Tables(0).Rows(0).Item("cc_transaction_code"), String.Empty)
                If (dsPaymentCashListItem.Tables(0).Rows(0).Item("cc_customer") Is String.Empty) Then
                    .CustomerPresent = False
                Else
                    .CustomerPresent = True
                End If
            End With

        End If

        Return oGetPaymentCashListItemDetailsResponse

    End Function

    ''' <summary>
    ''' This method gets the Receipt Cash List Items
    ''' </summary>
    Public Overloads Function GetReceiptCashListItems(ByVal oGetReceiptCashListItemsRequest As BaseGetReceiptCashListItemsRequestType) As BaseGetReceiptCashListItemsResponseType
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)
            Dim oResponse As BaseGetReceiptCashListItemsResponseType

            oResponse = GetReceiptCashListItems(con, oGetReceiptCashListItemsRequest)

            Return oResponse

        End Using

    End Function

    ''' <summary>
    '''This method is used to get receipt Cash List Items 
    '''</summary>
    '''<param name="oGetReceiptCashListItemsRequest" type="BaseGetReceiptCashListItemsRequestType"></param>
    '''<param name="con" type="SiriusConnection"></param>
    '''<remarks></remarks>
    Public Overloads Function GetReceiptCashListItems(ByVal con As SiriusConnection, ByVal oGetReceiptCashListItemsRequest As BaseGetReceiptCashListItemsRequestType) As BaseGetReceiptCashListItemsResponseType

        Dim oGetReceiptCashListItemsResponse As New BaseGetReceiptCashListItemsResponseType
        Dim oCoreBusiness As New CoreBusiness
        Dim oSAMErrorCollection As New SAMErrorCollection
        Dim dsCashListItem As DataSet = Nothing
        Dim dsResultDataSet As New DataSet
        Dim iCount As Integer
        Dim iBranchId As Integer
        Dim nTypeOfPackage As enumTypeOfPackage

        If oGetReceiptCashListItemsRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.GetReceiptCashListItemsRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oGetReceiptCashListItemsResponse = New SAMForInsuranceV2ImplementationTypes.GetReceiptCashListItemsResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If

        'Validate the mandatory parameters
        oGetReceiptCashListItemsRequest.Validate(CType(oSAMErrorCollection, Object))
        oSAMErrorCollection.CheckForErrors()

        ' Lookup Codes to ensure Valid BranchCode and CashListKey
        If Not String.IsNullOrEmpty(oGetReceiptCashListItemsRequest.BranchCode) Then
            iBranchId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
            PMLookupTable.Source, oGetReceiptCashListItemsRequest.BranchCode, "BranchCode", oSAMErrorCollection)
        End If
        oSAMErrorCollection.CheckForErrors()

        GetAndValidateSpecifiedTableCode(con, PMLookupTable.CashListItem, "cashlist_id", "cashlist_id", oGetReceiptCashListItemsRequest.CashListKey.ToString(), oSAMErrorCollection, "CashListKey")
        oSAMErrorCollection.CheckForErrors()

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_SelAll_CashListItem")
            cmd.AddInParameter("@cashlist_id", SqlDbType.Int).Value = oGetReceiptCashListItemsRequest.CashListKey
            dsCashListItem = con.ExecuteDataSet(cmd, "Row")
        End Using
        If dsCashListItem IsNot Nothing AndAlso dsCashListItem.Tables.Count > 0 AndAlso dsCashListItem.Tables(0).Rows.Count > 0 Then
            With dsCashListItem.Tables(0)
                dsResultDataSet.Tables.Add()
                dsResultDataSet.Tables(0).Columns.Add("CashListItemKey", GetType(System.Int32))
                dsResultDataSet.Tables(0).Columns.Add("MediaReference", GetType(System.String))
                dsResultDataSet.Tables(0).Columns.Add("MediaType", GetType(System.String))
                dsResultDataSet.Tables(0).Columns.Add("Amount", GetType(System.Double))
                dsResultDataSet.Tables(0).Columns.Add("AccountShortCode", GetType(System.String))
                dsResultDataSet.Tables(0).Columns.Add("Status", GetType(System.String))
                dsResultDataSet.Tables(0).Columns.Add("Letter", GetType(System.Boolean))
                For iCount = 0 To .Rows.Count - 1
                    dsResultDataSet.Tables(0).Rows.Add()
                    dsResultDataSet.Tables(0).Rows(iCount)("CashListItemKey") = Cast.ToInt32(.Rows(iCount).Item("CashListItemKey"), 0)
                    dsResultDataSet.Tables(0).Rows(iCount)("MediaReference") = Cast.ToString(.Rows(iCount).Item("MediaReference"), String.Empty)
                    dsResultDataSet.Tables(0).Rows(iCount)("MediaType") = Cast.ToString(.Rows(iCount).Item("MediaType"), String.Empty)
                    dsResultDataSet.Tables(0).Rows(iCount)("Amount") = Cast.ToDouble(.Rows(iCount).Item("Amount"), 0)
                    dsResultDataSet.Tables(0).Rows(iCount)("AccountShortCode") = Cast.ToString(.Rows(iCount).Item("AccountShortCode"), String.Empty)
                    dsResultDataSet.Tables(0).Rows(iCount)("Status") = Cast.ToString(.Rows(iCount).Item("Status"), String.Empty)
                    dsResultDataSet.Tables(0).Rows(iCount)("Letter") = Cast.ToBoolean(.Rows(iCount).Item("Letter"), False)
                Next

            End With
        Else
            Return oGetReceiptCashListItemsResponse
        End If
        If oGetReceiptCashListItemsRequest.WCFSecurityToken = "" Then
            Dim Docxml As New System.Xml.XmlDocument
            dsResultDataSet.DataSetName = "BaseGetReceiptCashListItemsResponseTypeReceiptCashListItems"
            dsResultDataSet.Tables(0).TableName = "Row"
            Docxml.LoadXml(dsResultDataSet.GetXml)
            If (dsResultDataSet.Tables.Count >= 1) Then
                oGetReceiptCashListItemsResponse.ResultDataset = Docxml.DocumentElement()
            End If
        End If
        oGetReceiptCashListItemsResponse.ResultData = dsResultDataSet
        Return oGetReceiptCashListItemsResponse

    End Function

    Private Function GetCashListForReceipt(ByVal con As SiriusConnection, ByVal icashListKey As Integer) As BaseCoreCashListType
        Dim dsCashList As New DataSet
        Dim oBusiness As New CoreBusiness
        Dim oBaseReceiptCashListType As New BaseReceiptCashListType
        Dim oErrors As New SAMErrorCollection

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_ACT_Select_CashList")
            cmd.AddInParameter("@cashlist_id", SqlDbType.Int).Value = Cast.NullIfDefault(icashListKey, Nothing)
            dsCashList = con.ExecuteDataSet(cmd, "CashList")
        End Using

        If dsCashList IsNot Nothing AndAlso dsCashList.Tables.Count > 0 AndAlso dsCashList.Tables(0).Rows.Count > 0 Then

            With oBaseReceiptCashListType
                .TypeCode = oBusiness.GetListItemFromID(Core.STSListType.PMLookup, PMLookupTable.CashListType, Convert.ToInt32(dsCashList.Tables(0).Rows(0).Item("cashlisttype_id")))
                .BankAccountCode = oBusiness.GetListItemFromID(Core.STSListType.PMLookup, "BankAccount", Convert.ToInt32(dsCashList.Tables(0).Rows(0).Item("bankaccount_id")))
                .CurrencyCode = oBusiness.GetListItemFromID(Core.STSListType.PMLookup, PMLookupTable.Currency, Convert.ToInt32(dsCashList.Tables(0).Rows(0).Item("currency_id")))
                .StatusCode = oBusiness.GetListItemFromID(Core.STSListType.PMLookup, PMLookupTable.CashListStatus, Convert.ToInt32(dsCashList.Tables(0).Rows(0).Item("cashliststatus_id")))
                .Reference = Cast.ToString(dsCashList.Tables(0).Rows(0).Item("cashlist_ref"), String.Empty)
                .ListDate = Cast.ToDateTime(dsCashList.Tables(0).Rows(0).Item("list_date"), Date.MinValue)
                .CashListKey = icashListKey
            End With
        End If

        Return oBaseReceiptCashListType

    End Function

    ''' <summary>
    '''This method is used to Authorise Claim payment
    '''<param name="oBaseAuthoriseClaimPaymentRequest" type ="BaseAuthoriseClaimPaymentRequestType">An object of SiriusConnection class</param>
    '''<returns>BaseAuthoriseClaimPaymentResponseType</returns>
    ''' </summary>
    Public Overloads Function AuthoriseClaimPayment(ByVal oBaseAuthoriseClaimPaymentRequest As BaseAuthoriseClaimPaymentRequestType) As BaseAuthoriseClaimPaymentResponseType
        Dim oResponse As BaseAuthoriseClaimPaymentResponseType
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)
            oResponse = AuthoriseClaimPayment(con, oBaseAuthoriseClaimPaymentRequest)

        End Using

        Return oResponse
    End Function

    Public Overloads Function GetAccountShortCodeFromParty(ByVal v_iPartyCnt As Integer) As String
        Dim sAccountShortCode As String = Nothing
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)
            GetAccountShortCodeFromParty(con, v_iPartyCnt, sAccountShortCode)

        End Using
        Return sAccountShortCode.Trim

    End Function

    ''' <summary>
    '''This method is used to Authorise Claim payment
    '''<param name="con" type ="BaseAuthoriseClaimPaymentRequestType">An object of SiriusConnection class</param>
    '''<param name="oAuthoriseClaimPaymentRequest" type ="BaseAuthoriseClaimPaymentRequestType"></param>
    '''<returns>BaseAuthoriseClaimPaymentResponseType</returns>
    ''' </summary>
    Public Overloads Function AuthoriseClaimPayment(ByVal con As SiriusConnection, ByVal oAuthoriseClaimPaymentRequest As BaseAuthoriseClaimPaymentRequestType) As BaseAuthoriseClaimPaymentResponseType
        Dim oCoreBusiness As New CoreBusiness
        Dim oErrors As New SAMErrorCollection
        Dim nbranchId As Integer
        Dim nTypeOfPackage As enumTypeOfPackage

        Dim dsClaimInfo As DataSet

        Dim oAuthoriseClaimPaymentResponse As SAMForInsuranceV2ImplementationTypes.AuthoriseClaimPaymentResponseType

        Dim iRet As Integer

        Dim is_referred As Integer
        Dim iTaskid As Integer
        Dim iTaskGroupid As Integer
        Dim iGroupid As Integer
        Dim sResult As String
        Dim sClientname As String = String.Empty
        Dim sDescription As String
        Dim itaskInstanceCnt As Integer
        Dim lReturn As Integer
        Dim obCLMAuthorisePayments As New bCLMAuthorisePayments.StepAuthorization
        Dim dsClaim As DataSet
        Dim iInsuranceHolderCnt As Integer
        Dim iInsuranceFolderCnt As Integer
        Dim iInsuranceFileCnt As Integer
        Dim sMessage As String
        Dim vKeyArray(1, 1) As Object
        Dim vkeyArrayObject As Object = Nothing
        Dim dAmount As Decimal
        Dim iCreatorUserKey As Integer
        Dim sClaimNumber As String
        Dim sCurrencyCode As String = String.Empty
        Dim iClaimKey As Integer
        Dim sOriginalUser As String = ""
        Dim nRecommendedBy As Integer
        Dim nCreatedBy As Integer
        Dim sClaimStatus As String
        Dim bDeclined As Boolean
        Dim nAssignTaskTo As Integer = _SiriusUser.UserID
        Const KCurExcDiff = 5
        'Following line is being added by rk as part of SAM Interop conversions and as per internal and SSP discussion in place of LOC next to it (now commented somewhere below saying oDatabase = con.PMDAODatabase)
        'con = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)

        Dim oDatabase As Object = Nothing '= con.SqlConnection.Database

        If Not con Is Nothing Then
            oDatabase = con.PMDAODatabase
        End If

        If oAuthoriseClaimPaymentRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.AuthoriseClaimPaymentRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oAuthoriseClaimPaymentResponse = New SAMForInsuranceV2ImplementationTypes.AuthoriseClaimPaymentResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If

        lReturn = CInt(obCLMAuthorisePayments.Initialise(
                              _SiriusUser.Username,
                              _SiriusUser.Password,
                              _SiriusUser.UserID,
                              _SiriusUser.SourceID,
                              _SiriusUser.LanguageID,
                              _SiriusUser.CurrencyID,
                              1,
                              SiriusUserDefaults.AppName,
                              vDatabase:=oDatabase))

        Dim oBusiness As New bCLMAuthorisePayments.Business

        lReturn = CInt(oBusiness.Initialise(
                                      _SiriusUser.Username,
                                      _SiriusUser.Password,
                                      _SiriusUser.UserID,
                                      _SiriusUser.SourceID,
                                      _SiriusUser.LanguageID,
                                      _SiriusUser.CurrencyID,
                                      1,
                                      SiriusUserDefaults.AppName,
                                      vDatabase:=oDatabase))

        Dim obCLMChangeClaimStatus As New bCLMChangeClaimStatus.Business

        lReturn = CInt(obCLMChangeClaimStatus.Initialise(
                                                      _SiriusUser.Username,
                                                      _SiriusUser.Password,
                                                      _SiriusUser.UserID,
                                                      _SiriusUser.SourceID,
                                                      _SiriusUser.LanguageID,
                                                      _SiriusUser.CurrencyID,
                                                      1,
                                                      SiriusUserDefaults.AppName,
                                                      vDatabase:=oDatabase))

        'Validate the mandatory parameters
        oAuthoriseClaimPaymentRequest.Validate(CType(oErrors, Object))
        oErrors.CheckForErrors()

        Try
            nbranchId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.Source, oAuthoriseClaimPaymentRequest.BranchCode, "BranchCode")
        Catch ex As Exception
            oErrors.AddInvalidData(SAMInvalidData.InvalidLookupListValue,
                                        SAMInvalidData.InvalidLookupListValue.ToString,
                                        "BranchCode",
                                        oAuthoriseClaimPaymentRequest.BranchCode)
        End Try

        oErrors.CheckForErrors()

        Try


            sResult = oCoreBusiness.GetProductOption(SIRHiddenOptions.SIROPTMultiStepApproval, nbranchId)
            oErrors.CheckForErrors()

            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_CLM_Get_Claim_Payment")

                cmd.AddInParameter("@ClaimPaymentKey", SqlDbType.Int).Value = oAuthoriseClaimPaymentRequest.ClaimPaymentKey
                dsClaimInfo = con.ExecuteDataSet(cmd, "ClaimDetails")
                If dsClaimInfo IsNot Nothing AndAlso dsClaimInfo.Tables.Count > 0 And dsClaimInfo.Tables(0).Rows.Count > 0 Then
                    dAmount = Cast.ToDecimal(dsClaimInfo.Tables(0).Rows(0)("Amount"), 0)
                    iCreatorUserKey = Cast.ToInt32(dsClaimInfo.Tables(0).Rows(0)("CreatorUserKey"), 0)
                    sClaimNumber = dsClaimInfo.Tables(0).Rows(0)("ClaimNumber").ToString()
                    iClaimKey = Cast.ToInt32(dsClaimInfo.Tables(0).Rows(0)("ClaimKey"), 0)
                    sCurrencyCode = dsClaimInfo.Tables(0).Rows(0)("CurrencySymbol").ToString()
                End If

            End Using
            Dim nBaseClaimKey As Integer = 0
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_CLM_Get_Base_Claim")
                Dim dsClaimDetails As DataSet = Nothing
                cmd.AddInParameter("@Claim_id", SqlDbType.Int).Value = iClaimKey

                dsClaimDetails = con.ExecuteDataSet(cmd, "Row")

                Dim dr As DataRow = dsClaimDetails.Tables(0).Rows(0)
                If dsClaimDetails.Tables(0).Rows.Count > 0 Then

                    nBaseClaimKey = Cast.ToInt32(dr.Item("base_claim_id"), 0)

                End If
            End Using

            con.BeginTransaction()
            oCoreBusiness.CheckSAMTSAndLock(Con:=con, BranchCode:=oAuthoriseClaimPaymentRequest.BranchCode,
                 Lockname:=CoreBusiness.LockName.ClaimId,
                 LockValue:=nBaseClaimKey,
                 TStamp:=oAuthoriseClaimPaymentRequest.TimeStamp)

            Dim dsReferredClaims As DataSet
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_CLM_Get_ReferredClaim_status")
                cmd.AddInParameter("@Claim_payment_ID", SqlDbType.Int).Value = oAuthoriseClaimPaymentRequest.ClaimPaymentKey
                dsReferredClaims = con.ExecuteDataSet(cmd, "Table")
                If Not (dsReferredClaims Is Nothing) Then
                    If dsReferredClaims.Tables(0).Rows.Count > 0 Then
                        is_referred = Cast.ToInt32(dsReferredClaims.Tables(0).Rows(0).Item(0), 0)
                        nRecommendedBy = Cast.ToInt32(dsReferredClaims.Tables(0).Rows(0).Item(2), 0)
                        nCreatedBy = Cast.ToInt32(dsReferredClaims.Tables(0).Rows(0).Item(3), 0)
                    End If
                End If
            End Using

            'Business Validation
            If is_referred = 0 OrElse is_referred = 2 Then
                oErrors.AddInvalidData(SAMConstants.SAMBusinessErrors.InvalidReferred,
                                                                "Claim Status can not be referred",
                                                               "Isreferred")
            End If
            oErrors.CheckForErrors()

            If oAuthoriseClaimPaymentRequest.ExclusiveLock Then
                AddExlusiveLock(con,
                                oCoreBusiness,
                                CoreBusiness.LockName.ClaimPayment,
                                oAuthoriseClaimPaymentRequest.ClaimPaymentKey,
                                0,
                                oAuthoriseClaimPaymentRequest.SessionValue,
                                _SiriusUser.UserID,
                                oAuthoriseClaimPaymentRequest.BranchCode)
            End If

            If oAuthoriseClaimPaymentRequest.IsRecommended Then
                If oAuthoriseClaimPaymentRequest.RecommendedBy IsNot Nothing AndAlso (Not String.IsNullOrEmpty(oAuthoriseClaimPaymentRequest.RecommendedBy)) Then
                    oAuthoriseClaimPaymentResponse.ErrorMessage = "You cannot recommend this payment, as there are 1 prior payments awaiting recommendation for this claim"
                ElseIf nCreatedBy = CInt(_SiriusUser.UserID) Then
                    oAuthoriseClaimPaymentResponse.ErrorMessage = "Cannot Proceed - Unable to recommend claim payments raised by yourself"
                Else
                    Dim dReserveAmount As Decimal = 0
                    Dim dRecommenderCurrAmount As Decimal = 0
                    Dim oGetUserAuthorityValueRequest As New BaseGetUserAuthorityValueRequestType
                    Dim oGetUserAuthorityValueResponse As New BaseGetUserAuthorityValueResponseType
                    With oGetUserAuthorityValueRequest
                        .SourceId = nbranchId
                        .UserCode = _SiriusUser.Username
                        .UserAuthorityOption = UserAuthorityOptions.IsRecommender
                        .BranchCode = oAuthoriseClaimPaymentRequest.BranchCode
                    End With
                    oGetUserAuthorityValueResponse = GetUserAuthorityValue(con, oGetUserAuthorityValueRequest)
                    If oGetUserAuthorityValueResponse.Errors Is Nothing Then
                        If oGetUserAuthorityValueResponse.UserAuthorityValue = "1" Then
                            dRecommenderCurrAmount = Cast.ToDecimal(oGetUserAuthorityValueResponse.UserAuthorityOptionalValue2, 0)
                            GetReserveTotalForClaimPayment(con, oAuthoriseClaimPaymentRequest.ClaimPaymentKey, dReserveAmount)
                            If dRecommenderCurrAmount < dReserveAmount Then
                                oAuthoriseClaimPaymentResponse.ErrorMessage = "Recommend limit is less than Gross Incurred on Claim."
                            End If
                        Else
                            oAuthoriseClaimPaymentResponse.ErrorMessage = "User Don’t have Recommend Authority"
                        End If

                    Else
                        oErrors.CheckForErrors()
                    End If
                End If

            Else
                If oAuthoriseClaimPaymentRequest.Declined = False AndAlso nRecommendedBy = CInt(_SiriusUser.UserID) Then
                    oAuthoriseClaimPaymentResponse.ErrorMessage = "Cannot Proceed - Unable to Authorize claim payments recommended by yourself"
                Else
                    sResult = oCoreBusiness.GetProductOption(SIRHiddenOptions.SIROPTMultiStepApproval, 1)
                    oErrors.CheckForErrors()

                    ' if error we need to unlock
                    If sResult = "1" AndAlso nRecommendedBy = 0 Then

                        obCLMAuthorisePayments.PaymentType = 1
                        obCLMAuthorisePayments.PaymentID = oAuthoriseClaimPaymentRequest.ClaimPaymentKey
                        obCLMAuthorisePayments.PaymentAmount = dAmount
                        obCLMAuthorisePayments.PaymentCreatorUserID = iCreatorUserKey

                        If oAuthoriseClaimPaymentRequest.Declined Then
                            iRet = obCLMAuthorisePayments.ProcessDecline()
                            If (iRet <> PMEReturnCode.PMTrue) AndAlso (iRet <> PMReturnCode.PMNotFound) Then
                                RaiseComMethodException("bCLMAuthorisePayments.ProcessDecline", iRet)

                            Else
                                bDeclined = True
                            End If
                        Else
                            iRet = obCLMAuthorisePayments.ProcessApproval()
                            If (iRet <> PMEReturnCode.PMTrue) AndAlso (iRet <> PMReturnCode.PMNotFound) Then
                                RaiseComMethodException("obCLMAuthorisePayments.ProcessApproval", iRet)
                            End If

                        End If
                        Dim sUserGroupCode As String = ""
                        Dim sErrorMessage As String = ""

                        If Not obCLMAuthorisePayments.LastStep Then
                            iGroupid = obCLMAuthorisePayments.GetStepGroupCode(r_sGroupCode:=sUserGroupCode, r_sErrorMessage:=sErrorMessage)
                        End If



                        If Cast.ToString(obCLMAuthorisePayments.ProcessErrorMessage, "") <> "" Then
                            If Cast.ToString(sUserGroupCode, "") = "" Then
                                oErrors.AddInvalidData(SAMInvalidData.DebtorUserGroupsAreNotSetup,
                                                                                  SAMInvalidData.DebtorUserGroupsAreNotSetup.ToString,
                                                                                   obCLMAuthorisePayments.ProcessErrorMessage)
                            Else
                                oErrors.AddInvalidData(SAMInvalidData.UserUnconfirmedExceptions,
                                                                                   SAMInvalidData.UserUnconfirmedExceptions.ToString,
                                                                                    obCLMAuthorisePayments.ProcessErrorMessage)
                            End If
                            oErrors.CheckForErrors()

                        End If
                        iRet = oBusiness.ProcessWTM(iClaimKey)
                        If (iRet <> PMEReturnCode.PMTrue) And (iRet <> PMReturnCode.PMNotFound) Then
                            'SAMFunc.DestroyCOMInterop(CObj(oBusiness))

                            If oAuthoriseClaimPaymentRequest.Declined = False Then
                                RaiseComMethodException("Business.ProcessWTM.Authorise", iRet)

                            Else
                                RaiseComMethodException("Business.ProcessWTM.Decline", iRet)

                            End If

                        End If

                        If obCLMAuthorisePayments.LastStep = True Then
                            If oAuthoriseClaimPaymentRequest.Declined = False Then
                                sMessage = "Your Claim Payment for Claim Number: " & sClaimNumber & " for the amount " &
                                           Trim$(sCurrencyCode) & " " & dAmount.ToString("0.##") & " has been authorized."
                            Else
                                sMessage = "Your Claim Payment for Claim Number: " & sClaimNumber & " for the amount " &
                                           Trim$(sCurrencyCode) & " " & dAmount.ToString("0.##") & " has been declined."
                            End If

                        Else
                            If oAuthoriseClaimPaymentRequest.Declined = False Then
                                sMessage = "Authorise Claim Payment for Claim Number: " & sClaimNumber & " for the amount of " &
                                           Trim$(sCurrencyCode) & " " & dAmount.ToString("0.##")
                            Else
                                sMessage = "Decline Claim Payment for Claim Number: " & sClaimNumber & " for the amount of " &
                                           Trim$(sCurrencyCode) & " " & dAmount.ToString("0.##")
                            End If

                        End If

                        dsClaim = getAuthoriseclaimDetail(iClaimKey, con)

                        If dsClaim IsNot Nothing AndAlso dsClaim.Tables.Count > 0 And dsClaim.Tables(0).Rows.Count > 0 Then
                            iInsuranceFileCnt = Cast.ToInt32(dsClaim.Tables(0).Rows(0)("Insurance_file_cnt"), 0)
                            iInsuranceFolderCnt = Cast.ToInt32(dsClaim.Tables(0).Rows(0)("insurance_folder_cnt"), 0)
                            iInsuranceHolderCnt = Cast.ToInt32(dsClaim.Tables(0).Rows(0)("insurance_holder_cnt"), 0)
                            sClientname = dsClaim.Tables(0).Rows(0)("insured_name").ToString()
                        End If
                        iTaskGroupid = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, "pmwrk_task_group", "CLAIMADM", "pmwrk_task_group_id", oErrors)

                        If oAuthoriseClaimPaymentRequest.Declined = False Then
                            If obCLMAuthorisePayments.LastStep = True Then
                                iTaskid = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, "pmwrk_task", "MEMO", "pmwrk_task_id", oErrors)
                                nAssignTaskTo = iCreatorUserKey
                            Else
                                iTaskid = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, "pmwrk_task", "AUTHPMNT", "pmwrk_task_id", oErrors)
                                nAssignTaskTo = iCreatorUserKey
                            End If
                        Else
                            iTaskid = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, "pmwrk_task", "MEMO", "pmwrk_task_id", oErrors)
                            nAssignTaskTo = nCreatedBy
                        End If
                        nAssignTaskTo = iCreatorUserKey
                        If sErrorMessage <> "" Then
                            iGroupid = iTaskGroupid
                        End If

                        If sErrorMessage <> "" Then
                            iGroupid = iTaskGroupid
                        End If

                        Dim nSourceId As Integer
                        Dim nCurrencyId As Integer

                        ' load currency and source from specified branch code
                        GetSourceDetails(oAuthoriseClaimPaymentRequest.BranchCode, nSourceId, nCurrencyId)

                        If iGroupid = 0 Then
                            iGroupid = iTaskGroupid
                        End If
                        
                        CreateTaskInstance(con, itaskInstanceCnt, iTaskGroupid, iTaskid,
                        sClientname, Date.Today, iGroupid, nAssignTaskTo,
                        sMessage, PMEWrkManTaskStatus.pmeWMTSNew, 0, Date.Today,
                        _SiriusUser.UserID, Date.Today, nAssignTaskTo, 1, String.Empty, 0, nSourceId)

                        bDeclined = False
                    Else
                        obCLMAuthorisePayments.LastStep = True
                    End If

                    If obCLMAuthorisePayments.LastStep = True Then

                        If oAuthoriseClaimPaymentRequest.Declined = False Then

                            iRet = obCLMChangeClaimStatus.SetProcessModes(vTransactionType:="C_CP")
                            If (iRet <> PMEReturnCode.PMTrue) And (iRet <> PMReturnCode.PMNotFound) Then
                                If oAuthoriseClaimPaymentRequest.Declined = False Then
                                    RaiseComMethodException("bCLMChangeClaimStatus.SetProcessModes.Authorise", iRet)
                                Else
                                    RaiseComMethodException("bCLMChangeClaimStatus.SetProcessModes.Decline", iRet)
                                End If
                            End If
                            obCLMChangeClaimStatus.ClaimId = iClaimKey
                            iRet = obCLMChangeClaimStatus.ChangeClaimStatusForSAM()
                            If (iRet <> PMEReturnCode.PMTrue) And (iRet <> PMReturnCode.PMNotFound) Then

                                If oAuthoriseClaimPaymentRequest.Declined = False Then
                                    RaiseComMethodException("bCLMChangeClaimStatus.Start.Authorise", iRet)
                                Else
                                    RaiseComMethodException("bCLMChangeClaimStatus.Start.Decline", iRet)
                                End If

                            End If
                        End If
                        dsClaim = getAuthoriseclaimDetail(iClaimKey, con)
                        If dsClaim IsNot Nothing AndAlso dsClaim.Tables.Count > 0 AndAlso dsClaim.Tables(0).Rows.Count > 0 Then
                            iInsuranceFileCnt = Cast.ToInt32(dsClaim.Tables(0).Rows(0)("insurance_file_cnt"), 0)
                            iInsuranceFolderCnt = Cast.ToInt32(dsClaim.Tables(0).Rows(0)("insurance_folder_cnt"), 0)
                            iInsuranceHolderCnt = Cast.ToInt32(dsClaim.Tables(0).Rows(0)("insurance_holder_cnt"), 0)
                            sClientname = dsClaim.Tables(0).Rows(0)("insured_name").ToString()
                            sOriginalUser = dsClaim.Tables(0).Rows(0)("username").ToString()
                        End If

                        sDescription = "Payment Created by - " & sOriginalUser & " and "
                        If oAuthoriseClaimPaymentRequest.Declined Then
                            sDescription = sDescription + "Declined by - " & _SiriusUser.Username &
                                           " for Claim Payment for amount " & Trim$(sCurrencyCode) & " " & dAmount.ToString("0.##")
                        Else
                            sDescription = sDescription + "Authorised by - " & _SiriusUser.Username &
                                           " for Claim Payment for amount " & Trim$(sCurrencyCode) & " " & dAmount.ToString("0.##")
                        End If
                        If Trim$(oAuthoriseClaimPaymentRequest.Comments) <> "" Then
                            sDescription = sDescription & ", Comments - " & Trim(oAuthoriseClaimPaymentRequest.Comments)
                        End If

                        If oAuthoriseClaimPaymentRequest.Declined Then
                            sClaimStatus = EventTypeCode.ClaimDecline
                        Else
                            sClaimStatus = EventTypeCode.ClaimAuthorise
                        End If

                        CreateDefaultClaimEvent(con, iInsuranceHolderCnt, iInsuranceFolderCnt, iInsuranceFileCnt, iClaimKey, "C_CP", _SiriusUser.UserID, sDescription, sClaimStatus)

                        If oAuthoriseClaimPaymentRequest.Declined Then

                            sDescription = oAuthoriseClaimPaymentRequest.Comments
                        Else

                            If Trim$(oAuthoriseClaimPaymentRequest.Comments) = "" Then
                                sDescription = "Payment of Claim for the amount " & Trim$(sCurrencyCode) & dAmount.ToString("0.##") & " - Payment Authorised"
                            Else
                                sDescription = "Payment of Claim for the amount " & Trim$(sCurrencyCode) & dAmount.ToString("0.##") & " - Payment Authorised ,Comments - " & oAuthoriseClaimPaymentRequest.Comments
                            End If
                        End If

                        iRet = obCLMChangeClaimStatus.UpdateClaimDesc(iClaimKey, sDescription)

                        If (iRet <> PMEReturnCode.PMTrue) And (iRet <> PMReturnCode.PMNotFound) Then
                            If oAuthoriseClaimPaymentRequest.Declined = False Then
                                RaiseComMethodException("bCLMChangeClaimStatus.UpdateClaimDesc.Authorise", iRet)
                            Else
                                RaiseComMethodException("bCLMChangeClaimStatus.UpdateClaimDesc.Decline", iRet)
                            End If

                        End If

                        If oAuthoriseClaimPaymentRequest.Declined = False Then
                            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_clm_Process_Authorise")
                                cmd.AddInParameter("@claim_id", SqlDbType.Int).Value = iClaimKey
                                con.ExecuteNonQuery(cmd)
                            End Using
                        Else
                            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_clm_Process_decline")
                                cmd.AddInParameter("@claim_id", SqlDbType.Int).Value = iClaimKey
                                cmd.AddInParameter("@payment_id", SqlDbType.Int).Value = oAuthoriseClaimPaymentRequest.ClaimPaymentKey
                                con.ExecuteNonQuery(cmd)
                            End Using
                            bDeclined = True
                        End If
                    End If
                End If
            End If
            If oAuthoriseClaimPaymentRequest.Declined = True AndAlso bDeclined = True And sResult <> "1" Then
                iRet = oBusiness.ProcessWTM(iClaimKey)
                If (iRet <> PMEReturnCode.PMTrue) AndAlso (iRet <> PMReturnCode.PMNotFound) Then
                    RaiseComMethodException("Business.ProcessWTM.Decline", iRet)
                End If

                If obCLMAuthorisePayments.LastStep = True Then
                    sMessage = "Your Claim Payment for Claim Number: " & sClaimNumber & " for the amount " & dAmount.ToString("0.##") & " has been declined."
                Else
                    sMessage = "Decline Claim Payment for Claim Number: " & sClaimNumber & " for the amount of " & dAmount.ToString("0.##")
                End If

                If sMessage = "" Then
                    sMessage = sDescription
                End If

                dsClaim = getAuthoriseclaimDetail(iClaimKey, con)

                If dsClaim IsNot Nothing AndAlso dsClaim.Tables.Count > 0 AndAlso dsClaim.Tables(0).Rows.Count > 0 Then
                    iInsuranceFileCnt = Cast.ToInt32(dsClaim.Tables(0).Rows(0)("Insurance_file_cnt"), 0)
                    iInsuranceFolderCnt = Cast.ToInt32(dsClaim.Tables(0).Rows(0)("insurance_folder_cnt"), 0)
                    iInsuranceHolderCnt = Cast.ToInt32(dsClaim.Tables(0).Rows(0)("insurance_holder_cnt"), 0)
                    sClientname = dsClaim.Tables(0).Rows(0)("insured_name").ToString()
                End If
                iTaskGroupid = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, "pmwrk_task_group", "CLAIMADM", "pmwrk_task_group_id", oErrors)
                iTaskid = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, "pmwrk_task", "MEMO", "pmwrk_task_id", oErrors)
                nAssignTaskTo = nCreatedBy

                Dim nUserGroupId As Integer
                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_User_Group_id")
                    cmd.AddInParameter("@nuser_id", SqlDbType.Int).Value = nAssignTaskTo
                    cmd.AddOutParameter("@npmuser_group_id", SqlDbType.Int)
                    con.ExecuteNonQuery(cmd)

                    nUserGroupId = Convert.ToInt32(cmd.Parameters("@npmuser_group_id").Value)
                End Using

                Dim nSourceId As Integer
                Dim nCurrencyId As Integer

                ' load currency and source from specified branch code
                GetSourceDetails(oAuthoriseClaimPaymentRequest.BranchCode, nSourceId, nCurrencyId)

                CreateTaskInstance(con, itaskInstanceCnt, iTaskGroupid, iTaskid,
            sClientname, Date.Today, nUserGroupId, nAssignTaskTo,
            sMessage, PMEWrkManTaskStatus.pmeWMTSNew, 0, Date.Today,
              _SiriusUser.UserID, Date.Today, _SiriusUser.UserID, 1, String.Empty, 0, nSourceId)



                Dim nLiveClaimStatusId As Integer = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.ClaimStatus, ClaimStatus.LiveOpenClaim, "ClaimStatusCode")
                UpdateClaimStatus(con, iClaimKey, nLiveClaimStatusId)

            End If


            Dim oImpRequest As New SAMForInsuranceV2ImplementationTypes.CreatePaymentCashListWithItemsRequestType
            Dim oImpResponse As New SAMForInsuranceV2ImplementationTypes.CreatePaymentCashListWithItemsResponseType

            Dim oGetUnallocatedClaimPaymentRequest As New SAMForInsuranceV2ImplementationTypes.GetUnallocatedClaimPaymentsRequestType
            Dim oGetUnallocatedClaimPaymentResponse As New SAMForInsuranceV2ImplementationTypes.GetUnallocatedClaimPaymentsResponseType
            Dim dsUnallocatedClaimPaymentsResponse As New DataSet

            Dim oGetAccountDetailsRequest As New SAMForInsuranceV2ImplementationTypes.GetAccountDetailsRequestType
            Dim oGetAccountDetailsResponse As New SAMForInsuranceV2ImplementationTypes.GetAccountDetailsResponseType

            Dim oGetTransactionDetailsRequest As New SAMForInsuranceV2ImplementationTypes.GetTransactionDetailsRequestType
            Dim oGetTransactionDetailsResponse As New SAMForInsuranceV2ImplementationTypes.GetTransactionDetailsResponseType
            Dim dsTransactionDetailsResponse As New DataSet

            Dim oUpdateAllocationRequest As New SAMForInsuranceV2ImplementationTypes.UpdateAllocationRequestType
            Dim oUpdateAllocationResponse As New SAMForInsuranceV2ImplementationTypes.UpdateAllocationResponseType


            If oAuthoriseClaimPaymentRequest.PaymentCashList IsNot Nothing AndAlso oAuthoriseClaimPaymentRequest.Declined = False Then

                oImpRequest.BranchCode = oAuthoriseClaimPaymentRequest.BranchCode
                oImpRequest.LoginUserName = oAuthoriseClaimPaymentRequest.LoginUserName
                oImpRequest.SourceId = oAuthoriseClaimPaymentRequest.SourceId
                oImpRequest.PaymentCashList = New BaseImplementationTypes.BasePaymentCashListType
                oImpRequest.PaymentCashList = oAuthoriseClaimPaymentRequest.PaymentCashList

                oImpResponse = DirectCast(CreatePaymentCashListWithItems(con, oImpRequest), SAMForInsuranceV2ImplementationTypes.CreatePaymentCashListWithItemsResponseType)

                oGetUnallocatedClaimPaymentRequest.AccountKey = oAuthoriseClaimPaymentRequest.AccountKey
                oGetUnallocatedClaimPaymentRequest.AccountKeySpecified = oAuthoriseClaimPaymentRequest.AccountKeySpecified
                oGetUnallocatedClaimPaymentRequest.BranchCode = oAuthoriseClaimPaymentRequest.BranchCode
                oGetUnallocatedClaimPaymentRequest.LoginUserName = oAuthoriseClaimPaymentRequest.LoginUserName
                oGetUnallocatedClaimPaymentRequest.PaymentDate = oAuthoriseClaimPaymentRequest.PaymentDate
                oGetUnallocatedClaimPaymentRequest.PaymentDateSpecified = oAuthoriseClaimPaymentRequest.PaymentDateSpecified
                oGetUnallocatedClaimPaymentRequest.PaymentDateTo = oAuthoriseClaimPaymentRequest.PaymentDateTo
                oGetUnallocatedClaimPaymentRequest.PaymentDateToSpecified = oAuthoriseClaimPaymentRequest.PaymentDateSpecified
                oGetUnallocatedClaimPaymentRequest.ShortCode = oAuthoriseClaimPaymentRequest.ShortCode
                oGetUnallocatedClaimPaymentRequest.ShortCodeSpecified = oAuthoriseClaimPaymentRequest.ShortCodeSpecified
                oGetUnallocatedClaimPaymentRequest.SourceId = oAuthoriseClaimPaymentRequest.SourceId
                oGetUnallocatedClaimPaymentResponse = DirectCast(GetUnallocatedClaimPayments(con:=con, oGetUnallocatedClaimPaymentsRequest:=oGetUnallocatedClaimPaymentRequest, dsUnallocatedClaimPaymentsResponse:=dsUnallocatedClaimPaymentsResponse), SAMForInsuranceV2ImplementationTypes.GetUnallocatedClaimPaymentsResponseType)

                Dim nRowCount As Integer = CInt(dsUnallocatedClaimPaymentsResponse.Tables(0).Rows.Count - 1)
                For i As Integer = 0 To dsUnallocatedClaimPaymentsResponse.Tables(0).Rows.Count - 1
                    If oAuthoriseClaimPaymentRequest.ClaimPaymentKey = CInt(dsUnallocatedClaimPaymentsResponse.Tables(0).Rows(i).Item("BaseClaimPaymentKey")) Then
                        nRowCount = i
                    End If
                Next


                Dim nAccountKey As Integer = CInt(dsUnallocatedClaimPaymentsResponse.Tables(0).Rows(nRowCount).Item("AccountKey"))
                Dim dUnallocatedAmount As Double = CDbl(dsUnallocatedClaimPaymentsResponse.Tables(0).Rows(nRowCount).Item("Amount"))

                oGetAccountDetailsRequest.DocumentRef = CStr(dsUnallocatedClaimPaymentsResponse.Tables(0).Rows(nRowCount).Item("DocumentRef"))
                oGetAccountDetailsRequest.AccountKey = nAccountKey
                oGetAccountDetailsRequest.AccountKeySpecified = True
                oGetAccountDetailsRequest.BranchCode = oAuthoriseClaimPaymentRequest.BranchCode
                oGetAccountDetailsRequest.LoginUserName = oAuthoriseClaimPaymentRequest.LoginUserName
                oGetAccountDetailsRequest.SourceArray = oAuthoriseClaimPaymentRequest.SourceArray

                oGetAccountDetailsResponse = DirectCast(GetAccountDetails(con, oGetAccountDetailsRequest:=oGetAccountDetailsRequest), SAMForInsuranceV2ImplementationTypes.GetAccountDetailsResponseType)

                Dim nRowCnt As Integer = oGetAccountDetailsResponse.Transactions.Row.GetUpperBound(0)
                oGetTransactionDetailsRequest.Allocation = New BaseImplementationTypes.BaseGetTransactionDetailsRequestTypeAllocation
                If oGetAccountDetailsResponse.Transactions.Row IsNot Nothing Then

                    Dim iCount As Integer = 0
                    For Each oImpRow As BaseImplementationTypes.BaseGetAccountDetailsResponseTypeTransactionsRow In oGetAccountDetailsResponse.Transactions.Row
                        ReDim Preserve oGetTransactionDetailsRequest.Allocation.Row(iCount)
                        oGetTransactionDetailsRequest.Allocation.Row(iCount) = New BaseImplementationTypes.BaseGetTransactionDetailsRequestTypeAllocationRow
                        oGetTransactionDetailsRequest.Allocation.Row(iCount).AllocationTransDetailKey = oGetAccountDetailsResponse.Transactions.Row(iCount).TransDetailKey
                        iCount = iCount + 1
                    Next

                End If
                oGetTransactionDetailsRequest.BranchCode = oAuthoriseClaimPaymentRequest.BranchCode
                oGetTransactionDetailsRequest.AccountKey = nAccountKey
                oGetTransactionDetailsRequest.AccountKeySpecified = True

                oGetTransactionDetailsResponse = DirectCast(GetTransactionDetails(con:=con, oGetTransactionDetailsRequest:=oGetTransactionDetailsRequest, dsTransactionDetailsResponse:=dsTransactionDetailsResponse), SAMForInsuranceV2ImplementationTypes.GetTransactionDetailsResponseType)

                For iCount As Integer = 0 To CInt(dsTransactionDetailsResponse.Tables(0).Rows.Count) - 1
                    ReDim Preserve oUpdateAllocationRequest.Allocation(iCount)
                    oUpdateAllocationRequest.Allocation(iCount) = New BaseImplementationTypes.BaseUpdateAllocationRequestTypeAllocation
                    oUpdateAllocationRequest.Allocation(iCount).AllocationAmount = CDbl(dsTransactionDetailsResponse.Tables(0).Rows(iCount).Item("Amount"))
                    oUpdateAllocationRequest.Allocation(iCount).AllocationAmountSpecified = True
                    oUpdateAllocationRequest.Allocation(iCount).AllocationTimeStamp = Cast.ToByteArray(dsTransactionDetailsResponse.Tables(0).Rows(iCount).Item("AllocationTimeStamp"))
                    oUpdateAllocationRequest.Allocation(iCount).AllocationTransdetailKey = CInt(dsTransactionDetailsResponse.Tables(0).Rows(iCount).Item("TransDetailKey"))
                Next
                oUpdateAllocationRequest.BranchCode = oAuthoriseClaimPaymentRequest.BranchCode
                oUpdateAllocationRequest.AccountKey = nAccountKey
                oUpdateAllocationRequest.CashListItemKey = oImpResponse.CashListItem(0).CashListItemKey
                oUpdateAllocationRequest.Amount = -dUnallocatedAmount
                oUpdateAllocationRequest.TransdetailKey = oImpResponse.CashListItem(0).TransDetailKey

                Dim dCurrencyDiff As Double = GetCurrencyDiff(con, oImpResponse.CashListItem(), CDbl(dsUnallocatedClaimPaymentsResponse.Tables(0).Rows(0).Item("CurrencyBaseXrate")))
                If (Math.Round(dCurrencyDiff, 2) <> 0) Then
                    oUpdateAllocationRequest.CurrencyDiff = Math.Round(dCurrencyDiff, 2)
                    oUpdateAllocationRequest.WriteOffReason = KCurExcDiff
                End If
                oUpdateAllocationResponse = DirectCast(UpdateAllocation(con:=con, oUpdateAllocationRequest:=oUpdateAllocationRequest), SAMForInsuranceV2ImplementationTypes.UpdateAllocationResponseType)
                oAuthoriseClaimPaymentResponse.AllocationStatus = oUpdateAllocationResponse.AllocationStatus
            End If

            con.CommitTransaction()

            'Delete an Exclusive Lock
            DeleteExlusiveLock(con,
                               oCoreBusiness,
                               CoreBusiness.LockName.ClaimPayment,
                               oAuthoriseClaimPaymentRequest.ClaimPaymentKey,
                               oAuthoriseClaimPaymentRequest.BranchCode)


            oCoreBusiness.UnlockAndGetSAMTS(
                       con,
                       oAuthoriseClaimPaymentRequest.BranchCode,
                       CoreBusiness.LockName.ClaimId,
                       nBaseClaimKey,
                       oAuthoriseClaimPaymentResponse.TimeStamp)

        Catch ex As Exception
            con.RollbackTransaction()
            'Delete an Exclusive Lock
            DeleteExlusiveLock(con,
                               oCoreBusiness,
                               CoreBusiness.LockName.ClaimPayment,
                               oAuthoriseClaimPaymentRequest.ClaimPaymentKey,
                               oAuthoriseClaimPaymentRequest.BranchCode)
            Throw
        Finally
            If obCLMAuthorisePayments IsNot Nothing Then
                obCLMAuthorisePayments.Dispose()
                obCLMAuthorisePayments = Nothing
            End If
            If oBusiness IsNot Nothing Then
                oBusiness.Dispose()
                oBusiness = Nothing
            End If

            If obCLMChangeClaimStatus IsNot Nothing Then
                obCLMChangeClaimStatus.Dispose()
                obCLMChangeClaimStatus = Nothing
            End If

        End Try
        Return oAuthoriseClaimPaymentResponse
    End Function

    ''' <summary>
    ''' GetReserveTotalForClaimPayment
    ''' </summary>
    ''' <param name="con"></param>
    ''' <param name="iClaimPaymentId"></param>
    ''' <param name="dResults"></param>
    ''' <remarks></remarks>
    Public Sub GetReserveTotalForClaimPayment(ByVal con As SiriusConnection,
                        ByVal iClaimPaymentId As Integer,
                        ByRef dResults As Decimal)

        Dim dsClaim As DataSet
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_Get_Reserve_For_Claim_Payment")
            cmd.AddInParameter("@Claim_Payment_Id", SqlDbType.Int).Value = iClaimPaymentId
            dsClaim = con.ExecuteDataSet(cmd, "Claims")
        End Using
        dResults = 0
        If dsClaim IsNot Nothing Then
            If dsClaim.Tables(0).Rows.Count > 0 Then
                dResults = Cast.ToDecimal(dsClaim.Tables(0).Rows(0).Item(0), 0)
            End If
        End If
    End Sub

    ''' <summary>
    '''This method is used to getClaim payment
    '''<param name="con" type ="SiriusConnection">An object of SiriusConnection class</param>
    '''<param name="ClaimKey" type ="integer"></param>
    '''<returns>Dataset</returns>
    ''' </summary>
    Public Function getAuthoriseclaimDetail(ByVal ClaimKey As Integer, ByVal con As SiriusConnection) As DataSet
        Dim dsClaim As DataSet
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_get_claim_cnts")

            cmd.AddInParameter("@claim_id", SqlDbType.Int).Value = ClaimKey
            dsClaim = con.ExecuteDataSet(cmd, "Claims")

        End Using
        Return dsClaim
    End Function

    Public Overloads Sub GetAccountShortCodeFromParty(ByVal con As SiriusConnection,
                        ByVal v_iPartyCnt As Integer,
                        ByRef v_sAccountShortCode As String)

        Dim dsParty As DataSet
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_ACT_Get_Account_ShortCode_From_PartyCnt")
            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = v_iPartyCnt
            dsParty = con.ExecuteDataSet(cmd, "Account")
        End Using
        If dsParty IsNot Nothing Then
            If dsParty.Tables(0).Rows.Count > 0 Then
                v_sAccountShortCode = dsParty.Tables(0).Rows(0).Item(0).ToString
            End If
        End If
    End Sub

#Region "Reverse Allocation"
    Public Overloads Function ReverseAllocation(ByVal ReverseAllocationRequest As BaseReverseAllocationRequestType) As BaseReverseAllocationResponseType

        ' validate the request structure against the specified business rules
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)

            Dim oResponse As BaseReverseAllocationResponseType

            oResponse = ReverseAllocation(con, ReverseAllocationRequest)

            Return oResponse

        End Using
    End Function

    Public Overloads Function ReverseAllocation(ByVal con As SiriusConnection, ByVal oReverseAllocationRequest As BaseReverseAllocationRequestType) As BaseReverseAllocationResponseType

        Dim oReverseAllocationResponse As New BaseReverseAllocationResponseType
        Dim oCoreBusiness As New CoreBusiness
        Dim oErrors As New SAMErrorCollection
        Dim oAllocate As New bACTAllocationPost.Automated
        Dim iBranchId As Integer
        Dim iAllocDays As Integer
        Dim iRet As Integer

        Dim nTypeOfPackage As enumTypeOfPackage

        If oReverseAllocationRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.ReverseAllocationRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oReverseAllocationResponse = New SAMForInsuranceV2ImplementationTypes.ReverseAllocationResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If

        'Mandatory validations
        oReverseAllocationRequest.Validate(CObj(oErrors))
        oErrors.CheckForErrors()

        Try
            iBranchId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.Source, oReverseAllocationRequest.BranchCode, "BranchCode")
        Catch ex As Exception
            oErrors.AddInvalidData(SAMInvalidData.InvalidLookupListValue,
                                        SAMInvalidData.InvalidLookupListValue.ToString,
                                        "BranchCode",
                                        oReverseAllocationRequest.BranchCode)
        End Try
        oErrors.CheckForErrors()


        Dim oGetUserAuthorityValueRequest As New BaseGetUserAuthorityValueRequestType
        Dim oGetUserAuthorityValueResponse As New BaseGetUserAuthorityValueResponseType
        With oGetUserAuthorityValueRequest
            .SourceId = iBranchId
            .UserCode = _SiriusUser.Username
            .UserAuthorityOption = UserAuthorityOptions.AllowReverseAllocation
            .BranchCode = oReverseAllocationRequest.BranchCode
        End With
        oGetUserAuthorityValueResponse = GetUserAuthorityValue(con, oGetUserAuthorityValueRequest)
        If oGetUserAuthorityValueResponse.Errors Is Nothing Then
            If oGetUserAuthorityValueResponse.UserAuthorityValue = "1" Then
                'Get Transaction Details
                Dim dtTransactionDate As DateTime
                Dim sTransationSpare As String = ""
                GetTransactionDetail(con,
                   oReverseAllocationRequest.TransDetailKey,
                   dtTransactionDate,
                   sTransationSpare)

                iAllocDays = Cast.ToInt32(oGetUserAuthorityValueResponse.UserAuthorityOptionalValue1, 0)

                If dtTransactionDate.AddDays(iAllocDays) < DateTime.Today Then
                    oErrors.AddBusinessRule(SAMBusinessErrors.UserDoesNotHaveReverseAllocationAuthority,
                                                            "Unable to reverse allocation, time limit exceeded")
                End If

                If sTransationSpare.ToUpper().StartsWith("REVER") Then
                    oErrors.AddBusinessRule(SAMBusinessErrors.CannotReverseAllocate,
                            "Cannot reverse an allocation on a reversed/reversal transaction")
                End If
            Else
                oErrors.AddBusinessRule(SAMBusinessErrors.UserDoesNotHaveReverseAllocationAuthority,
                                                            "Cannot reverse an allocation , user has no rights")
            End If

            oErrors.CheckForErrors()
        Else
            oErrors.CheckForErrors()
        End If

        'This portion will call the com method to initialize
        Dim oDatabase As Object = con.PMDAODatabase()

        iRet = CInt(oAllocate.Initialise(
                              _SiriusUser.Username,
                              _SiriusUser.Password,
                              _SiriusUser.UserID,
                              _SiriusUser.SourceID,
                              _SiriusUser.LanguageID,
                              _SiriusUser.CurrencyID,
                              1,
                              SiriusUserDefaults.AppName,
                              vDatabase:=oDatabase))

        If (iRet <> PMEReturnCode.PMTrue) Then

            ' if the account processing fails then throw a business rule error
            oErrors.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed,
                                                SAMBusinessErrors.COMComponentMethodFailed.ToString,
                                                "oAllocate.Initialise")
            oErrors.CheckForErrors()

        End If


        If Not oReverseAllocationRequest.IgnoreWarnings Then
            Dim bDoAllocationDetailPairsExist As Boolean
            iRet = oAllocate.DoAllocationDetailPairsExist(
                          r_bDoAllocationDetailPairsExist:=bDoAllocationDetailPairsExist,
                          v_lTransDetailID:=oReverseAllocationRequest.TransDetailKey,
                          v_lAllocationID:=oReverseAllocationRequest.AllocationKey)
            If iRet <> PMEReturnCode.PMTrue Then
                ' if the account processing fails then throw a business rule error
                oErrors.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed,
                                                    SAMBusinessErrors.COMComponentMethodFailed.ToString,
                                                    "bSIRAllocate.Business.DoAllocationDetailPairsExist Failed")
                oErrors.CheckForErrors()
            End If

            If Not bDoAllocationDetailPairsExist Then
                oReverseAllocationResponse.Warnings = "It is not possible to reverse just this transaction. All transactions within the same allocation must be reversed together"
            Else
                oReverseAllocationResponse.Warnings = ""
            End If
        Else
            oReverseAllocationResponse.Warnings = ""
        End If

        If oReverseAllocationResponse.Warnings.Length = 0 OrElse oReverseAllocationRequest.IgnoreWarnings Then
            iRet = oAllocate.ReverseAllocation(
                          v_lTransDetailID:=oReverseAllocationRequest.TransDetailKey,
                          v_lAllocationID:=oReverseAllocationRequest.AllocationKey)
            If iRet <> PMEReturnCode.PMTrue Then
                ' if the account processing fails then throw a business rule error
                oErrors.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed,
                                                    SAMBusinessErrors.COMComponentMethodFailed.ToString,
                                                    "bSIRAllocate.Business.ReverseAllocation Failed")
                oErrors.CheckForErrors()
            End If

        End If
        oAllocate.Dispose()
        oAllocate = Nothing
        Return oReverseAllocationResponse

    End Function

    Private Sub GetTransactionDetail(ByVal con As SiriusConnection,
                                            ByVal lTransDetailId As Integer,
                                            ByRef dtTransactionDate As Date,
                                            ByRef sSpare As String)
        Dim ds As DataSet = Nothing

        Try
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_Transaction_Details")
                cmd.AddInParameter("@TransDetail_Id", SqlDbType.Int).Value = lTransDetailId
                ds = con.ExecuteDataSet(cmd, "Table")
                If ds IsNot Nothing Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        With ds.Tables(0).Rows(0)
                            dtTransactionDate = Cast.ToDateTime(.Item("effective_date"), Date.MinValue)
                            sSpare = Cast.ToString(.Item("spare"), String.Empty)
                        End With
                    End If
                End If
            End Using

        Catch ex As Exception
            Throw
        End Try

    End Sub

    ''' <summary>
    ''' GetPaymentTypeCashListItem Connection Creation
    ''' </summary>
    ''' <param name="oGetPaymentTypeCashListItemRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function GetPaymentTypeCashListItem(ByVal oGetPaymentTypeCashListItemRequest As BaseGetPaymentTypeCashListItemRequestType) As BaseGetPaymentTypeCashListItemResponseType
        ' validate the request structure against the specified business rules
        Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
            Dim oResponse As BaseGetPaymentTypeCashListItemResponseType
            oResponse = GetPaymentTypeCashListItem(con, oGetPaymentTypeCashListItemRequest)
            Return oResponse
        End Using
    End Function

    ''' <summary>
    ''' GetPaymentTypeCashListItem definition
    ''' </summary>
    ''' <param name="con"></param>
    ''' <param name="oGetPaymentTypeCashListItemRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function GetPaymentTypeCashListItem(ByVal con As SiriusConnection, ByVal oGetPaymentTypeCashListItemRequest As BaseGetPaymentTypeCashListItemRequestType) As BaseGetPaymentTypeCashListItemResponseType

        Dim oGetPaymentTypeCashListItemResponse As New BaseGetPaymentTypeCashListItemResponseType
        Dim oCoreBusiness As New CoreBusiness
        Dim oErrors As New SAMErrorCollection
        Dim dsCashListItem As DataSet = Nothing
        Dim icount As Int32 = 0
        Dim nbranchId As Integer
        Dim oCashList As New BasePaymentCashListType
        Dim dsCashList As DataSet
        Dim nTypeOfPackage As enumTypeOfPackage

        If oGetPaymentTypeCashListItemRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.GetPaymentTypeCashListItemRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oGetPaymentTypeCashListItemResponse = New SAMForInsuranceV2ImplementationTypes.GetPaymentTypeCashListItemResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If

        ' exit if there are any missing parameters
        oGetPaymentTypeCashListItemRequest.Validate(CObj(oErrors))
        oErrors.CheckForErrors()

        ' Lookup Codes to ensure they are valid – To be done for all codes in the request, e.g.
        nbranchId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
          PMLookupTable.Source, oGetPaymentTypeCashListItemRequest.BranchCode, "Source", oErrors)
        oErrors.CheckForErrors()

        GetAndValidateSpecifiedTableCode(con, PMLookupTable.CashListItem, "cashlistitem_id", "cashlistitem_id", oGetPaymentTypeCashListItemRequest.CashListItemKey.ToString(), oErrors, "CashListItemKey")
        oErrors.CheckForErrors()
        Try
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_CashList")
                cmd.AddInParameter("@ncashlistitem_id", SqlDbType.Int).Value = oGetPaymentTypeCashListItemRequest.CashListItemKey
                dsCashList = con.ExecuteDataSet(cmd, "Row")
            End Using

            If dsCashList IsNot Nothing AndAlso dsCashList.Tables.Count > 0 AndAlso dsCashList.Tables(0).Rows.Count > 0 Then
                With dsCashList.Tables(0)
                    oCashList.CurrencyCode = GetAndValidateDescriptionById(con,
                                                                               PMLookupTable.Currency,
                                                                               "Code",
                                                                               "Currency_id",
                                                                               .Rows(icount).Item("currency_id").ToString).ToString.Trim()

                    oCashList.BankAccountCode = GetAndValidateDescriptionById(con,
                                                                       PMLookupTable.BankAccount,
                                                                       "Code",
                                                                       "bankaccount_id",
                                                                       .Rows(icount).Item("bankaccount_id").ToString).ToString.Trim()
                    oCashList.Reference = .Rows(icount).Item("cashlist_ref").ToString.Trim()
                    oCashList.StatusCode = GetAndValidateDescriptionById(con,
                                                                       PMLookupTable.CashListStatus,
                                                                       "Code",
                                                                       "cashliststatus_id",
                                                                       .Rows(icount).Item("cashliststatus_id").ToString).ToString.Trim()
                    oCashList.TypeCode = GetAndValidateDescriptionById(con,
                                                                       PMLookupTable.CashListType,
                                                                       "Code",
                                                                       "cashlisttype_id",
                                                                       .Rows(icount).Item("cashlisttype_id").ToString).ToString.Trim()
                    oCashList.ListDate = Cast.ToDateTime(.Rows(icount).Item("list_date"), Date.MinValue)
                    oCashList.BankAccountKey = Cast.ToInt32(.Rows(icount).Item("bankaccount_id"), 0)
                End With

                ReDim oGetPaymentTypeCashListItemResponse.CashList(0)
                oGetPaymentTypeCashListItemResponse.CashList(0) = oCashList
                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_ACT_SelAll_CashListItem")
                    cmd.AddInParameter("@ncashlistitem_id", SqlDbType.Int).Value = oGetPaymentTypeCashListItemRequest.CashListItemKey
                    dsCashListItem = con.ExecuteDataSet(cmd, "Row")
                End Using



                If dsCashListItem IsNot Nothing AndAlso dsCashListItem.Tables.Count > 0 AndAlso dsCashListItem.Tables(0).Rows.Count > 0 Then
                    With dsCashListItem.Tables(0)
                        For icount = 0 To .Rows.Count - 1
                            Dim oCashListItem As New BasePaymentCashListItemType
                            oCashListItem.AccountKey = Cast.ToInt32(.Rows(icount).Item("account_id"), 0)
                            oCashListItem.AccountShortCode = GetAndValidateDescriptionById(con,
                                                                           NonPMLookupTable.Account,
                                                                           "Short_Code",
                                                                           "Account_id",
                                                                           .Rows(icount).Item("account_id").ToString).ToString.Trim()

                            oCashListItem.AllocationStatusKey = Cast.ToInt32(.Rows(icount).Item("allocationstatus_id"), 0)
                            oCashListItem.AllocationStatusCode = GetAndValidateDescriptionById(con,
                                                                            "AllocationStatus",
                                                                            "code",
                                                                            "allocationstatus_id",
                                                                            oCashListItem.AllocationStatusKey.ToString()).ToString.Trim()

                            oCashListItem.StatusKey = Cast.ToInt32(.Rows(icount).Item("cashlistitem_receipt_status_id"), 0)
                            oCashListItem.StatusCode = GetAndValidateDescriptionById(con, "cashlistitem_receipt_status", "code", "cashlistitem_receipt_status_id", oCashListItem.StatusKey.ToString()).ToString.Trim()

                            oCashListItem.Amount = Cast.ToDecimal(.Rows(icount).Item("amount"), 0)
                            oCashListItem.CashListItemKey = Cast.ToInt32(.Rows(icount).Item("cashlistitem_id"), 0)
                            oCashListItem.ContactName = (.Rows(icount).Item("contact_name")).ToString.Trim()
                            oCashListItem.TypeCode = (.Rows(icount).Item("payment_type_code")).ToString.Trim()
                            oCashListItem.MediaReference = .Rows(icount).Item("media_ref").ToString.Trim()

                            oCashListItem.MediaTypeKey = Cast.ToInt32(.Rows(icount).Item("mediatype_id"), 0)
                            oCashListItem.MediaTypeCode = GetAndValidateDescriptionById(con,
                                                                           PMLookupTable.MediaType,
                                                                           "Code",
                                                                           "MediaType_id",
                                                                           .Rows(icount).Item("mediatype_id").ToString).Trim()


                            oCashListItem.OurReference = .Rows(icount).Item("our_ref").ToString.Trim()
                            oCashListItem.TheirReference = .Rows(icount).Item("their_ref").ToString.Trim()
                            oCashListItem.TransactionDate = Cast.ToDateTime(.Rows(icount).Item("transaction_date"), Date.MinValue)
                            oCashListItem.FurtherDetails = Cast.ToString(.Rows(icount).Item("receipt_details"), String.Empty).ToString.Trim()

                            Dim oBank As New BaseBankPaymentType
                            oBank.AccountCode = .Rows(icount).Item("payment_account_code").ToString.Trim()
                            oBank.BranchCode = .Rows(icount).Item("payment_branch_code").ToString.Trim()
                            oBank.PartyBankKey = Cast.ToInt32(.Rows(icount).Item("party_bank_id"), 0)
                            oBank.PayeeName = .Rows(icount).Item("payment_name").ToString.Trim()
                            oBank.Reference1 = .Rows(icount).Item("payment_reference1").ToString.Trim()
                            oBank.Reference2 = .Rows(icount).Item("payment_reference2").ToString.Trim()
                            oBank.BIC = .Rows(icount).Item("business_identifier_code").ToString.Trim()
                            oBank.IBAN = .Rows(icount).Item("international_bank_account_number").ToString.Trim()
                            oCashListItem.Bank = oBank

                            Dim oAddress As New BaseSimpleAddressType
                            oAddress.AddressLine1 = .Rows(icount).Item("address1").ToString.Trim()
                            oAddress.AddressLine2 = .Rows(icount).Item("address2").ToString.Trim()
                            oAddress.AddressLine3 = .Rows(icount).Item("address3").ToString.Trim()
                            oAddress.AddressLine4 = .Rows(icount).Item("address4").ToString.Trim()
                            oAddress.CountryKey = Cast.ToInt32(.Rows(icount).Item("address_country"), 0)
                            oAddress.CountryCode = GetAndValidateDescriptionById(con, "Country", "code", "country_id", .Rows(icount).Item("address_country").ToString).ToString.Trim()
                            oAddress.PostCode = .Rows(icount).Item("postal_code").ToString.Trim()
                            oCashListItem.ContactAddress = oAddress


                            Dim oCreditCard As New BaseCreditCardType
                            oCreditCard.Number = .Rows(icount).Item("cc_number").ToString.Trim()
                            oCreditCard.ExpiryDate = .Rows(icount).Item("cc_expiry_date").ToString.Trim()
                            oCreditCard.StartDate = .Rows(icount).Item("cc_start_date").ToString.Trim()
                            oCreditCard.Issue = .Rows(icount).Item("cc_issue").ToString.Trim()
                            oCreditCard.Pin = .Rows(icount).Item("cc_pin").ToString.Trim()
                            oCreditCard.AuthCode = .Rows(icount).Item("cc_auth_code").ToString.Trim()
                            oCreditCard.NameOnCreditCard = .Rows(icount).Item("cc_name").ToString.Trim()
                            oCreditCard.ManualAuthCode = .Rows(icount).Item("cc_manual_auth_code").ToString.Trim()
                            oCreditCard.TransactionCode = .Rows(icount).Item("cc_transaction_code").ToString.Trim()
                            If (.Rows(icount).Item("cc_customer") Is String.Empty) Then
                                oCreditCard.CustomerPresent = False
                            Else
                                oCreditCard.CustomerPresent = True
                            End If
                            oCashListItem.CreditCard = oCreditCard

                            ReDim Preserve oGetPaymentTypeCashListItemResponse.CashList(0).PaymentItem(icount)
                            oGetPaymentTypeCashListItemResponse.CashList(0).PaymentItem(icount) = oCashListItem
                        Next
                    End With
                End If
            End If

        Catch ex As Exception
            Throw
        Finally
            oCashList = Nothing
            oCoreBusiness = Nothing
        End Try

        Return oGetPaymentTypeCashListItemResponse

    End Function



    ''' <summary>
    ''' ValidatedWildcardSearch
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <param name="r_sFieldValue"></param>
    ''' <remarks></remarks>
    Private Sub ValidatedWildcardSearch(ByVal sBranchCode As String,
                                        ByRef r_sFieldValue As String, ByVal sFieldValueText As String)
        Dim oBusiness As New CoreBusiness
        Dim bDisableWildcardSearchOption As Boolean
        Dim sDisableWildcardSearchOption As String = ""
        Dim sEnablePartialWildcardSearchOption As String = ""
        Dim bEnablePartialWildcardSearchOption As Boolean
        Dim oSAMErrorCollection As New SAMErrorCollection

        Try


            oBusiness.GetSystemOption(sBranchCode, SystemOption.DisableWildcardSearch,
                                      sDisableWildcardSearchOption)

            oBusiness.GetSystemOption(sBranchCode, SystemOption.EnablePartialWildcardSearch,
                                      sEnablePartialWildcardSearchOption)

            If (Trim(sDisableWildcardSearchOption) = "1") Then
                bDisableWildcardSearchOption = True
            Else
                bDisableWildcardSearchOption = False
            End If

            If (Trim(sEnablePartialWildcardSearchOption) = "1") Then
                bEnablePartialWildcardSearchOption = True
            Else
                bEnablePartialWildcardSearchOption = False
            End If

            If Not oBusiness.ValidWildcardSearch(
                bDisableWildcardSearchOption,
                bEnablePartialWildcardSearchOption,
                r_sFieldValue) Then
                oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidWildcardSearch,
                                                   SAMInvalidData.InvalidWildcardSearch.ToString,
                                                   sFieldValueText)
            End If

            oSAMErrorCollection.CheckForErrors()

        Catch ex As Exception
            Dim STSErrorEx As New STSErrorPublisher("Failed to Validated Wildcard Search.", ex)
            STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), "ValidatedWildcardSearch", "ValidatedWildcardSearch", True)
        End Try
    End Sub

    ''' <summary>
    ''' FindPaymentDetails
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FindPaymentDetails(ByVal oRequest As BaseFindPaymentDetailsRequestType) As BaseFindPaymentDetailsResponseType

        Dim oBusiness As New CoreBusiness
        Dim oResponse As New BaseImplementationTypes.BaseFindPaymentDetailsResponseType
        Dim nTypeOfPackage As enumTypeOfPackage
        Dim iBankAccountId As Int32
        Dim iPaymentBranchId As Int32
        Dim iPaymentStatusId As Int32
        Dim iMediaTypeId As Int32
        Dim iPaymentTypeId As Int32
        Dim Docxml As System.Xml.XmlDocument

        If oRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.FindPaymentDetailsRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oResponse = New SAMForInsuranceV2ImplementationTypes.FindPaymentDetailsResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
        End If

        Dim oSAMErrorCollection As New SAMErrorCollection
        Dim iSourceId As Integer


        ' Check mandatory fields
        If String.IsNullOrEmpty(oRequest.BranchCode) Then
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
            SAMInvalidData.MandatoryInputMissing.ToString,
            "BranchCode")
        End If
        oSAMErrorCollection.CheckForErrors()

        ' Check wildcard searches

        ValidatedWildcardSearch(oRequest.BranchCode, oRequest.PayeeName, "PayeeName")
        ValidatedWildcardSearch(oRequest.BranchCode, oRequest.BatchReference, "BatchReference")
        ValidatedWildcardSearch(oRequest.BranchCode, oRequest.ClientAccountNumber, "ClientAccountNumber")
        ValidatedWildcardSearch(oRequest.BranchCode, oRequest.PolicyClaimNumber, "PolicyClaimNumber")

        ' Lookup Codes to ensure they are valid
        Try
            iSourceId = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.Source, oRequest.BranchCode, "BranchCode")
        Catch ex As Exception
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue,
            SAMInvalidData.InvalidLookupListValue.ToString,
            "BranchCode",
            oRequest.BranchCode)
        End Try
        oSAMErrorCollection.CheckForErrors()

        Try

            Dim dsFindPaymentDetails As DataSet = Nothing
            ' DATA VALIDATION
            Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                                _SiriusUser.Username, _SiriusUser.SourceID,
                                                _SiriusUser.LanguageID,
                                                SiriusUserDefaults.AppName)
                ' validate the request structure against the specified business rules

                If ValidateFindPaymentDetails(con, oBusiness, oRequest, iMediaTypeId, iPaymentBranchId, iPaymentStatusId, iBankAccountId, iPaymentTypeId, oSAMErrorCollection) = False Then
                    oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                               SAMInvalidData.MandatoryInputMissing.ToString,
                               "Invalid LookupList Value",
                               oRequest.BranchCode)
                End If

                ' throw any data related errors
                oSAMErrorCollection.CheckForErrors()

                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_ACT_Do_FindPayment")
                    With oRequest
                        If .PayeeName IsNot Nothing AndAlso String.IsNullOrEmpty(.PayeeName) = False Then
                            cmd.AddInParameter("@payee_name", SqlDbType.VarChar, 60).Value = GetPaymentName(.PayeeName)
                        End If
                        If iBankAccountId <> 0 Then
                            cmd.AddInParameter("@account_id", SqlDbType.Int).Value = iBankAccountId
                        End If
                        If iPaymentTypeId <> 0 Then
                            cmd.AddInParameter("@payment_type_id", SqlDbType.Int).Value = iPaymentTypeId
                        End If
                        If iMediaTypeId <> 0 Then
                            cmd.AddInParameter("@payment_media_type_id", SqlDbType.Int).Value = iMediaTypeId
                        End If
                        If iPaymentStatusId <> 0 Then
                            cmd.AddInParameter("@payment_status_id", SqlDbType.Int).Value = iPaymentStatusId
                        End If
                        If .BatchReference IsNot Nothing AndAlso String.IsNullOrEmpty(.BatchReference) = False Then
                            cmd.AddInParameter("@batch_ref", SqlDbType.VarChar, 100).Value = .BatchReference
                        End If
                        cmd.AddInParameter("@user_id", SqlDbType.SmallInt).Value = _SiriusUser.UserID
                        If .MaxRowsToFetchSpecified Then
                            cmd.AddInParameter("@MaxRowsToFetch", SqlDbType.Int).Value = .MaxRowsToFetch
                        End If
                        If .PaymentBranch IsNot Nothing AndAlso String.IsNullOrEmpty(.PaymentBranch) = False Then
                            cmd.AddInParameter("@branch", SqlDbType.VarChar, 30).Value = .PaymentBranch
                        End If
                        If .ClientCode IsNot Nothing AndAlso String.IsNullOrEmpty(.ClientCode) = False Then
                            cmd.AddInParameter("@clientcode", SqlDbType.VarChar, 50).Value = .ClientCode
                        End If
                        If .ClientAccountNumber IsNot Nothing AndAlso String.IsNullOrEmpty(.ClientAccountNumber) = False Then
                            cmd.AddInParameter("@client_account_number", SqlDbType.VarChar, 60).Value = .ClientAccountNumber
                        End If
                        If .PolicyClaimNumber IsNot Nothing AndAlso String.IsNullOrEmpty(.PolicyClaimNumber) = False Then
                            cmd.AddInParameter("@policy_claim_number", SqlDbType.VarChar, 60).Value = .PolicyClaimNumber
                        End If
                        If .MediaReferenceFrom IsNot Nothing AndAlso String.IsNullOrEmpty(.MediaReferenceFrom) = False Then
                            cmd.AddInParameter("@media_from ", SqlDbType.VarChar, 60).Value = .MediaReferenceFrom
                        End If
                        If .MediaReferenceTo IsNot Nothing AndAlso String.IsNullOrEmpty(.MediaReferenceTo) = False Then
                            cmd.AddInParameter("@media_to ", SqlDbType.VarChar, 60).Value = .MediaReferenceTo
                        End If
                        If .AmountRangeFromSpecified Then
                            cmd.AddInParameter("@amount_from", SqlDbType.Decimal).Value = .AmountRangeFrom
                        End If
                        If .AmountrangeToSpecified Then
                            cmd.AddInParameter("@amount_to", SqlDbType.Decimal).Value = .AmountrangeTo
                        End If
                        If .DateFromSpecified Then
                            cmd.AddInParameter("@date_from", SqlDbType.DateTime).Value = .DateFrom
                        End If
                        If .DateToSpecified Then
                            cmd.AddInParameter("@date_to", SqlDbType.DateTime).Value = .DateTo
                        End If
                        cmd.AddInParameter("@showonlyoutstanding", SqlDbType.Int).Value = .ShowOnlyOutStanding
                    End With
                    dsFindPaymentDetails = con.ExecuteDataSet(cmd, "PaymentDetails")
                End Using
            End Using

            If Not (dsFindPaymentDetails IsNot Nothing AndAlso dsFindPaymentDetails.Tables.Count > 0 AndAlso dsFindPaymentDetails.Tables(0).Rows.Count > 0) Then
                Return oResponse
            End If
            Docxml = New System.Xml.XmlDocument
            dsFindPaymentDetails.DataSetName = "BaseFindPaymentDetailsResponseTypePaymentDetails"
            dsFindPaymentDetails.Tables(0).TableName = "Row"
            Docxml.LoadXml(dsFindPaymentDetails.GetXml)

            oResponse.ResultDataset = Docxml.DocumentElement()


        Catch ex As Exception
            Dim STSErrorEx As New STSErrorPublisher("Failed to convert XML (ResultArray).", ex)
            STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), "FindPaymentDetails", "FindPaymentDetails", True)
        Finally
            If Not IsNothing(Docxml) Then
                Docxml = Nothing
            End If

        End Try

        Return oResponse
    End Function

    ''' <summary>
    ''' ValidateFindPaymentDetails
    ''' </summary>
    ''' <param name="con"></param>
    ''' <param name="oBusiness"></param>
    ''' <param name="oFindPaymentDetails"></param>
    ''' <param name="iMediatypeId"></param>
    ''' <param name="iPaymentBranchId"></param>
    ''' <param name="iPaymentSatusID"></param>
    ''' <param name="iBankAccountID"></param>
    ''' <param name="iPaymentTypeId"></param>
    ''' <param name="oSAMErrorCollection"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidateFindPaymentDetails(ByVal con As SiriusConnection, ByVal oBusiness As CoreBusiness, ByRef oFindPaymentDetails As BaseFindPaymentDetailsRequestType,
    ByRef iMediatypeId As Int32, ByRef iPaymentBranchId As Int32, ByRef iPaymentSatusID As Int32, ByRef iBankAccountID As Int32, ByRef iPaymentTypeId As Int32,
    ByRef oSAMErrorCollection As SAMErrorCollection) As Boolean

        Dim bIsCheque As Boolean

        Try

            ' Validate following  Request Parameter and return the valid ID where needed.
            bIsCheque = False

            If oFindPaymentDetails IsNot Nothing Then
                With oFindPaymentDetails
                    If .MediaType = "CQ" Then
                        bIsCheque = True
                    End If

                    If .PayeeName IsNot Nothing AndAlso Len(Convert.ToString(.PayeeName).Trim) <> 0 Then
                        If ValidateString(Convert.ToString(.PayeeName).Trim, "") = False Then
                            oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidFormat,
                                        SAMInvalidData.InvalidFormat.ToString,
                                        "PayeeName")
                            ValidateFindPaymentDetails = False
                            Exit Function
                        End If
                    End If

                    If .ClientAccountNumber IsNot Nothing AndAlso Len(Convert.ToString(.ClientAccountNumber).Trim) <> 0 Then
                        If ValidateString(Convert.ToString(.ClientAccountNumber).Trim, "") = False Then
                            oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidFormat,
                                        SAMInvalidData.InvalidFormat.ToString,
                                        "ClientAccountNumber")
                            ValidateFindPaymentDetails = False
                            Exit Function
                        End If
                    End If

                    If .PolicyClaimNumber IsNot Nothing AndAlso Len(Convert.ToString(.PolicyClaimNumber).Trim) <> 0 Then
                        If ValidateString(Convert.ToString(.PolicyClaimNumber).Trim, "/\-_") = False Then
                            oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidFormat,
                                       SAMInvalidData.InvalidFormat.ToString,
                                       "PolicyClaimNumber")
                            ValidateFindPaymentDetails = False
                            Exit Function
                        End If
                        If Informations.inStr(Convert.ToString(.PolicyClaimNumber).Trim, "/") = 1 Or Informations.inStr(Convert.ToString(.PolicyClaimNumber).Trim, "\") = 1 Or Informations.inStr(Convert.ToString(.PolicyClaimNumber).Trim, "-") = 1 Or Informations.inStr(Convert.ToString(.PolicyClaimNumber).Trim, "_") = 1 Then
                            oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidFormat,
                                       SAMInvalidData.InvalidFormat.ToString,
                                       "PolicyClaimNumber")
                            ValidateFindPaymentDetails = False
                            Exit Function
                        End If
                    End If

                    If .BatchReference IsNot Nothing AndAlso Len(Convert.ToString(.BatchReference).Trim) <> 0 Then
                        If ValidateString(Convert.ToString(.BatchReference).Trim, "") = False Then
                            oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidFormat,
                                        SAMInvalidData.InvalidFormat.ToString,
                                        "BatchReference")
                            ValidateFindPaymentDetails = False
                            Exit Function
                        End If
                    End If

                    If .AmountRangeFromSpecified AndAlso Len(Convert.ToString(.AmountRangeFrom).Trim) <> 0 Then
                        If IsNumeric(.AmountRangeFrom) <> True Then
                            oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidFormat,
                                            SAMInvalidData.InvalidFormat.ToString,
                                            "AmountRangeFrom")
                            ValidateFindPaymentDetails = False
                            Exit Function
                        End If
                    End If

                    If .AmountrangeToSpecified AndAlso Len(Convert.ToString(.AmountrangeTo).Trim) <> 0 Then
                        If IsNumeric(.AmountrangeTo) <> True Then
                            oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidFormat,
                                            SAMInvalidData.InvalidFormat.ToString,
                                            "AmountrangeTo")
                            ValidateFindPaymentDetails = False
                            Exit Function
                        End If
                    End If

                    If bIsCheque = True Then

                        If .MediaReferenceFrom IsNot Nothing AndAlso Len(Convert.ToString(.MediaReferenceFrom).Trim) <> 0 Then
                            If IsNumeric(.MediaReferenceFrom) <> True Or Len(Convert.ToString(.MediaReferenceFrom).Trim) > 10 Or Informations.inStr(Convert.ToString(.MediaReferenceFrom).Trim, ".") > 0 Then
                                oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidFormat,
                                                    SAMInvalidData.InvalidFormat.ToString,
                                                    "MediaReferenceFrom")
                                ValidateFindPaymentDetails = False
                                Exit Function
                            End If
                        End If

                        If .MediaReferenceTo IsNot Nothing AndAlso Len(Convert.ToString(.MediaReferenceTo).Trim) <> 0 Then
                            If IsNumeric(.MediaReferenceTo) <> True Or Len(Convert.ToString(.MediaReferenceTo).Trim) > 10 Or Informations.inStr(Convert.ToString(.MediaReferenceTo).Trim, ".") > 0 Then
                                oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidFormat,
                                                    SAMInvalidData.InvalidFormat.ToString,
                                                    "MediaReferenceTo")
                                ValidateFindPaymentDetails = False
                                Exit Function
                            End If
                        End If
                    Else
                        If .MediaReferenceFrom IsNot Nothing AndAlso Len(Convert.ToString(.MediaReferenceFrom).Trim) <> 0 Then
                            If ValidateString(Convert.ToString(.MediaReferenceFrom).Trim, "/\-_.") = False Then
                                oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidFormat,
                                                      SAMInvalidData.InvalidFormat.ToString,
                                                      "MediaReferenceFrom")
                                ValidateFindPaymentDetails = False
                                Exit Function
                            End If

                            If Informations.inStr(Convert.ToString(.MediaReferenceFrom).Trim, "/") = 1 Or Informations.inStr(Convert.ToString(.MediaReferenceFrom).Trim, "\") = 1 Or Informations.inStr(Convert.ToString(.MediaReferenceFrom).Trim, "-") = 1 Or Informations.inStr(Convert.ToString(.MediaReferenceFrom).Trim, "_") = 1 Or Informations.inStr(Convert.ToString(.MediaReferenceTo).Trim, ".") = 1 Then
                                oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidFormat,
                                                     SAMInvalidData.InvalidFormat.ToString,
                                                     "MediaReferenceFrom")
                                ValidateFindPaymentDetails = False
                                Exit Function
                            End If
                        End If
                        If .MediaReferenceTo IsNot Nothing AndAlso Len(Convert.ToString(.MediaReferenceTo).Trim) <> 0 Then
                            If ValidateString(Convert.ToString(.MediaReferenceTo).Trim, "/\-_.") = False Then
                                oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidFormat,
                                                   SAMInvalidData.InvalidFormat.ToString,
                                                   "MediaReferenceTo")
                                ValidateFindPaymentDetails = False
                                Exit Function
                            End If

                            If Informations.inStr(Convert.ToString(.MediaReferenceTo).Trim, "/") = 1 Or Informations.inStr(Convert.ToString(.MediaReferenceTo).Trim, "\") = 1 Or Informations.inStr(.MediaReferenceTo, "-") = 1 Or Informations.inStr(Convert.ToString(.MediaReferenceTo).Trim, "_") = 1 Or Informations.inStr(Convert.ToString(.MediaReferenceTo).Trim, ".") = 1 Then
                                oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidFormat,
                                                    SAMInvalidData.InvalidFormat.ToString,
                                                    "MediaReferenceTo")
                                ValidateFindPaymentDetails = False
                                Exit Function
                            End If

                            If Len(Convert.ToString(.PolicyClaimNumber).Trim) <> 0 Then
                                If ValidateString(Convert.ToString(.PolicyClaimNumber).Trim, "/\-_") = False Then
                                    oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidFormat,
                                                   SAMInvalidData.InvalidFormat.ToString,
                                                   "PolicyClaimNumber")
                                    ValidateFindPaymentDetails = False
                                    Exit Function
                                End If
                            End If
                        End If
                    End If

                    If .PaymentBranch IsNot Nothing AndAlso String.IsNullOrEmpty(.PaymentBranch) = False Then
                        iPaymentBranchId = oBusiness.GetAndValidateListItemFromCode(
                                                                            Core.STSListType.PMLookup,
                                                                            PMLookupTable.Source,
                                                                            .PaymentBranch,
                                                                            "PaymentBranch",
                                                                            oSAMErrorCollection)
                    End If
                    If .PaymentStatus IsNot Nothing AndAlso String.IsNullOrEmpty(.PaymentStatus) = False Then
                        iPaymentSatusID = oBusiness.GetAndValidateListItemFromCode(
                                                                            Core.STSListType.PMLookup,
                                                                            PMLookupTable.CashListItemPaymentStatus,
                                                                            .PaymentStatus,
                                                                            "PaymentStatus",
                                                                            oSAMErrorCollection)
                    End If
                    If .PaymentType IsNot Nothing AndAlso String.IsNullOrEmpty(.PaymentType) = False Then
                        iPaymentTypeId = oBusiness.GetAndValidateListItemFromCode(
                                                                            Core.STSListType.PMLookup,
                                                                            PMLookupTable.CashListItemPaymentType,
                                                                            .PaymentType,
                                                                            "PaymentType",
                                                                            oSAMErrorCollection)
                    End If
                    If .BankAccount IsNot Nothing AndAlso String.IsNullOrEmpty(.BankAccount) = False Then
                        iBankAccountID = oBusiness.GetAndValidateListItemFromCode(
                                                                            Core.STSListType.PMLookup,
                                                                            PMLookupTable.BankAccount,
                                                                            .BankAccount,
                                                                            "BankAccount",
                                                                            oSAMErrorCollection)
                    End If

                    If .MediaType IsNot Nothing AndAlso String.IsNullOrEmpty(.MediaType) = False Then
                        iMediatypeId = oBusiness.GetAndValidateListItemFromCode(
                                                                            Core.STSListType.PMLookup,
                                                                            PMLookupTable.MediaType,
                                                                            .MediaType,
                                                                            "MediaType",
                                                                            oSAMErrorCollection)
                    End If

                    oSAMErrorCollection.CheckForErrors()
                End With
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function

    ''' <summary>
    ''' ValidateString
    ''' </summary>
    ''' <param name="s_InputString"></param>
    ''' <param name="s_AllowedSpecialChars"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidateString(ByVal s_InputString As String, ByVal s_AllowedSpecialChars As String) As Boolean
        Dim iLen As Integer
        Dim iKeyAscii As Integer

        ValidateString = True
        For iLen = 1 To Len(s_InputString)
            iKeyAscii = Asc(Mid(s_InputString, iLen, 1))

            If (iKeyAscii < 48 Or iKeyAscii > 57) And (iKeyAscii < 65 Or iKeyAscii > 90) And (iKeyAscii < 97 Or iKeyAscii > 122) And iKeyAscii <> 37 Then
                If Informations.inStr(s_AllowedSpecialChars, Mid(s_InputString, iLen, 1)) > 0 Then
                    ValidateString = True
                Else
                    ValidateString = False
                    Exit For
                End If
            End If
        Next
    End Function

    ''' <summary>
    ''' CancelPayment
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CancelPayment(ByVal oRequest As BaseCancelPaymentRequestType) As BaseCancelPaymentResponseType

        Dim oBusiness As New CoreBusiness
        Dim oResponse As New BaseImplementationTypes.BaseCancelPaymentResponseType
        Dim nTypeOfPackage As enumTypeOfPackage
        Dim oACTDocumentReversal As bACTDocumentReversal.Business = Nothing
        Dim oACTPaymentMaintenance As bACTPaymentMaintenance.Form = Nothing
        Dim iSourceId As Integer
        Dim iComReturnValue As Integer

        If oRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.CancelPaymentRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oResponse = New SAMForInsuranceV2ImplementationTypes.CancelPaymentResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
        End If

        Dim oSAMErrorCollection As New SAMErrorCollection

        ' Lookup Codes to ensure they are valid
        Try
            iSourceId = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.Source, oRequest.BranchCode, "BranchCode")
        Catch ex As Exception
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue,
            SAMInvalidData.InvalidLookupListValue.ToString,
            "BranchCode",
            oRequest.BranchCode)
        End Try
        oSAMErrorCollection.CheckForErrors()

        '*******************
        ' DATA VALIDATION
        '*******************
        If oRequest.TransDetailKey = 0 Then
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
            SAMInvalidData.MandatoryInputMissing.ToString,
            "TransDetailKey")
        End If
        ' throw any data related errors
        oSAMErrorCollection.CheckForErrors()

        If oRequest.ReverseReasonKey = 0 Then
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
            SAMInvalidData.MandatoryInputMissing.ToString,
            "ReverseReasonKey")
        End If
        ' throw any data related errors
        oSAMErrorCollection.CheckForErrors()

        If oRequest.CashListItemKey = 0 Then
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
            SAMInvalidData.MandatoryInputMissing.ToString,
            "CashListItemKey")
        End If

        ' throw any data related errors
        oSAMErrorCollection.CheckForErrors()
        Try
            Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                                _SiriusUser.Username, _SiriusUser.SourceID,
                                                _SiriusUser.LanguageID,
                                                SiriusUserDefaults.AppName)

                oACTDocumentReversal = New bACTDocumentReversal.Business
                SAMFunc.InitialiseSBOObject(con, oACTDocumentReversal, _SiriusUser, oRequest.BranchCode, "bACTDocumentReversal.Business")

                oACTDocumentReversal.TransDetailId = oRequest.TransDetailKey
                oACTDocumentReversal.IsCashlistItemReversal = True

                iComReturnValue = oACTDocumentReversal.Start()

                oACTPaymentMaintenance = New bACTPaymentMaintenance.Form
                SAMFunc.InitialiseSBOObject(con, oACTPaymentMaintenance, _SiriusUser, oRequest.BranchCode, "bACTPaymentMaintenance.Form")

                iComReturnValue = oACTPaymentMaintenance.SetCashListItemFlags(
                v_lCashlistitem_id:=oRequest.CashListItemKey,
                v_dtReversed_date:=Date.Now,
                v_iCashlistitem_reverse_pmuser_id:=_SiriusUser.UserID,
                v_lCashlistitem_reverse_reason_id:=oRequest.ReverseReasonKey,
                v_lcashlistitem_reversal_transdetail_id:=oACTDocumentReversal.ReversalTransDetailId)
                If iComReturnValue <> PMEReturnCode.PMTrue AndAlso iComReturnValue <> PMEReturnCode.PMNotFound Then
                    RaiseComMethodException("bACTPaymentMaintenance.Form.SetCashListItemFlags", iComReturnValue)
                End If

            End Using

            Return oResponse

        Catch ex As Exception
            Throw
        Finally
            If oACTDocumentReversal IsNot Nothing Then
                oACTDocumentReversal.Dispose()
                oACTDocumentReversal = Nothing
            End If
            If oACTPaymentMaintenance IsNot Nothing Then
                oACTPaymentMaintenance.Dispose()
                oACTPaymentMaintenance = Nothing
            End If
        End Try
    End Function

    ''' <summary>
    ''' FindReceiptDetails
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FindReceiptDetails(ByVal oRequest As BaseFindReceiptDetailsRequestType) As BaseFindReceiptDetailsResponseType

        Dim oBusiness As New CoreBusiness
        Dim oResponse As New BaseImplementationTypes.BaseFindReceiptDetailsResponseType
        Dim nTypeOfPackage As enumTypeOfPackage
        Dim iBankAccountId As Int32
        Dim iReceiptBranchId As Int32
        Dim iReceiptStatusId As Int32
        Dim iMediaTypeId As Int32
        Dim dsFindReceiptDetails As DataSet = Nothing

        If oRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.FindReceiptDetailsRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oResponse = New SAMForInsuranceV2ImplementationTypes.FindReceiptDetailsResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
        End If

        Dim oSAMErrorCollection As New SAMErrorCollection
        Dim iSourceId As Integer


        ' Check mandatory fields
        If String.IsNullOrEmpty(oRequest.BranchCode) Then
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
            SAMInvalidData.MandatoryInputMissing.ToString,
            "BranchCode")
        End If
        oSAMErrorCollection.CheckForErrors()

        ' Check wildcard searches

        ValidatedWildcardSearch(oRequest.BranchCode, oRequest.PayeeName, "PayeeName")
        ValidatedWildcardSearch(oRequest.BranchCode, oRequest.BatchReference, "BatchReference")
        ValidatedWildcardSearch(oRequest.BranchCode, oRequest.ClientAccountNumber, "ClientAccountNumber")
        ValidatedWildcardSearch(oRequest.BranchCode, oRequest.PolicyClaimNumber, "PolicyClaimNumber")

        ' Lookup Codes to ensure they are valid
        Try
            iSourceId = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.Source, oRequest.BranchCode, "BranchCode")
        Catch ex As Exception
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue,
            SAMInvalidData.InvalidLookupListValue.ToString,
            "BranchCode",
            oRequest.BranchCode)
        End Try
        oSAMErrorCollection.CheckForErrors()

        Try
            ' DATA VALIDATION
            Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                                _SiriusUser.Username, _SiriusUser.SourceID,
                                                _SiriusUser.LanguageID,
                                                SiriusUserDefaults.AppName)

                If ValidateFindReceiptDetails(con, oBusiness, oRequest, iMediaTypeId, iReceiptBranchId, iReceiptStatusId, iBankAccountId, oSAMErrorCollection) = False Then
                    oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                               SAMInvalidData.MandatoryInputMissing.ToString,
                               "Invalid LookupList Value",
                               oRequest.BranchCode)
                End If

                ' throw any data related errors
                oSAMErrorCollection.CheckForErrors()

                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_ACT_Do_FindReceipt")
                    With oRequest
                        If .PayeeName IsNot Nothing AndAlso String.IsNullOrEmpty(.PayeeName) = False Then
                            cmd.AddInParameter("@payee_name", SqlDbType.VarChar, 60).Value = .PayeeName
                        End If
                        If iBankAccountId > 0 Then
                            cmd.AddInParameter("@account_id", SqlDbType.Int).Value = iBankAccountId
                        End If
                        If .DocumentReference IsNot Nothing AndAlso String.IsNullOrEmpty(.DocumentReference) = False Then
                            cmd.AddInParameter("@DocumentReference", SqlDbType.VarChar, 50).Value = .DocumentReference
                        End If
                        If iMediaTypeId <> 0 Then
                            cmd.AddInParameter("@Receipt_media_type_id", SqlDbType.Int).Value = iMediaTypeId
                        End If
                        If iReceiptStatusId <> 0 Then
                            cmd.AddInParameter("@Receipt_status_id", SqlDbType.Int).Value = iReceiptStatusId
                        End If
                        If .BatchReference IsNot Nothing AndAlso String.IsNullOrEmpty(.BatchReference) = False Then
                            cmd.AddInParameter("@batch_ref", SqlDbType.VarChar, 100).Value = .BatchReference
                        End If
                        cmd.AddInParameter("@user_id", SqlDbType.SmallInt).Value = _SiriusUser.UserID
                        If .MaxRowsToFetchSpecified Then
                            cmd.AddInParameter("@MaxRowsToFetch", SqlDbType.Int).Value = .MaxRowsToFetch
                        End If
                        If .ReceiptBranch IsNot Nothing AndAlso String.IsNullOrEmpty(.ReceiptBranch) = False Then
                            cmd.AddInParameter("@branch", SqlDbType.Int).Value = .ReceiptBranch
                        End If
                        If .ClientCode IsNot Nothing AndAlso String.IsNullOrEmpty(.ClientCode) = False Then
                            cmd.AddInParameter("@clientcode", SqlDbType.VarChar, 50).Value = .ClientCode
                        End If
                        If .ClientAccountNumber IsNot Nothing AndAlso String.IsNullOrEmpty(.ClientAccountNumber) = False Then
                            cmd.AddInParameter("@client_account_number", SqlDbType.VarChar, 60).Value = .ClientAccountNumber
                        End If
                        If .PolicyClaimNumber IsNot Nothing AndAlso String.IsNullOrEmpty(.PolicyClaimNumber) = False Then
                            cmd.AddInParameter("@policy_claim_number", SqlDbType.VarChar, 60).Value = .PolicyClaimNumber
                        End If
                        If .MediaReferenceFrom IsNot Nothing AndAlso String.IsNullOrEmpty(.MediaReferenceFrom) Then
                            cmd.AddInParameter("@media_from ", SqlDbType.VarChar, 60).Value = .MediaReferenceFrom
                        End If
                        If .MediaReferenceTo IsNot Nothing AndAlso String.IsNullOrEmpty(.MediaReferenceTo) Then
                            cmd.AddInParameter("@media_to ", SqlDbType.VarChar, 60).Value = .MediaReferenceTo
                        End If
                        If .AmountRangeFromSpecified Then
                            cmd.AddInParameter("@amount_from", SqlDbType.Decimal).Value = .AmountRangeFrom
                        End If
                        If .AmountrangeToSpecified Then
                            cmd.AddInParameter("@amount_to", SqlDbType.Decimal).Value = .AmountrangeTo
                        End If
                        If .DateFromSpecified Then
                            cmd.AddInParameter("@date_from", SqlDbType.DateTime).Value = .DateFrom
                        End If
                        If .DateToSpecified Then
                            cmd.AddInParameter("@date_to", SqlDbType.DateTime).Value = .DateTo
                        End If
                        cmd.AddInParameter("@showonlyoutstanding", SqlDbType.Int).Value = .ShowOnlyOutStanding
                    End With
                    dsFindReceiptDetails = con.ExecuteDataSet(cmd, "ReceiptDetails")
                End Using

            End Using

            If Not (dsFindReceiptDetails IsNot Nothing AndAlso dsFindReceiptDetails.Tables.Count > 0 AndAlso dsFindReceiptDetails.Tables(0).Rows.Count > 0) Then
                Return oResponse
            End If

            Dim Docxml As New System.Xml.XmlDocument
            dsFindReceiptDetails.DataSetName = "BaseFindReceiptDetailsResponseTypeReceiptDetails"
            dsFindReceiptDetails.Tables(0).TableName = "Row"
            Docxml.LoadXml(dsFindReceiptDetails.GetXml)

            oResponse.ResultDataset = Docxml.DocumentElement()

        Catch ex As Exception
            Dim STSErrorEx As New STSErrorPublisher("Failed to convert XML (ResultArray).", ex)
            STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), "FindReceiptDetails", "FindReceiptDetails", True)
        Finally
        End Try

        Return oResponse
    End Function

    ''' <summary>
    ''' ValidateFindReceiptDetails
    ''' </summary>
    ''' <param name="con"></param>
    ''' <param name="oBusiness"></param>
    ''' <param name="oFindReceiptDetails"></param>
    ''' <param name="iMediatypeId"></param>
    ''' <param name="iReceiptBranchId"></param>
    ''' <param name="iReceiptSatusID"></param>
    ''' <param name="iBankAccountID"></param>
    ''' <param name="oSAMErrorCollection"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidateFindReceiptDetails(ByVal con As SiriusConnection, ByVal oBusiness As CoreBusiness, ByRef oFindReceiptDetails As BaseFindReceiptDetailsRequestType,
ByRef iMediatypeId As Int32, ByRef iReceiptBranchId As Int32, ByRef iReceiptSatusID As Int32, ByRef iBankAccountID As Int32,
ByRef oSAMErrorCollection As SAMErrorCollection) As Boolean

        Dim bIsCheque As Boolean

        Try

            ' Validate following  Request Parameter and return the valid ID where needed.
            bIsCheque = False

            If oFindReceiptDetails IsNot Nothing Then
                With oFindReceiptDetails
                    If .MediaType = "CQ" Then
                        bIsCheque = True
                    End If

                    If .PayeeName IsNot Nothing AndAlso Len(Convert.ToString(.PayeeName).Trim) <> 0 Then
                        If ValidateString(Convert.ToString(.PayeeName).Trim, "") = False Then
                            oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidFormat,
                                        SAMInvalidData.InvalidFormat.ToString,
                                        "PayeeName")
                            ValidateFindReceiptDetails = False
                            Exit Function
                        End If
                    End If

                    If .ClientAccountNumber IsNot Nothing AndAlso Len(Convert.ToString(.ClientAccountNumber).Trim) <> 0 Then
                        If ValidateString(Convert.ToString(.ClientAccountNumber).Trim, "") = False Then
                            oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidFormat,
                                        SAMInvalidData.InvalidFormat.ToString,
                                        "ClientAccountNumber")
                            ValidateFindReceiptDetails = False
                            Exit Function
                        End If
                    End If

                    If .PolicyClaimNumber IsNot Nothing AndAlso Len(Convert.ToString(.PolicyClaimNumber).Trim) <> 0 Then
                        If ValidateString(Convert.ToString(.PolicyClaimNumber).Trim, "/\-_") = False Then
                            oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidFormat,
                                       SAMInvalidData.InvalidFormat.ToString,
                                       "PolicyClaimNumber")
                            ValidateFindReceiptDetails = False
                            Exit Function
                        End If
                        If Informations.inStr(Convert.ToString(.PolicyClaimNumber).Trim, "/") = 1 Or Informations.inStr(Convert.ToString(.PolicyClaimNumber).Trim, "\") = 1 Or Informations.inStr(Convert.ToString(.PolicyClaimNumber).Trim, "-") = 1 Or Informations.inStr(Convert.ToString(.PolicyClaimNumber).Trim, "_") = 1 Then
                            oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidFormat,
                                       SAMInvalidData.InvalidFormat.ToString,
                                       "PolicyClaimNumber")
                            ValidateFindReceiptDetails = False
                            Exit Function
                        End If
                    End If

                    If .BatchReference IsNot Nothing AndAlso Len(Convert.ToString(.BatchReference).Trim) <> 0 Then
                        If ValidateString(Convert.ToString(.BatchReference).Trim, "") = False Then
                            oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidFormat,
                                        SAMInvalidData.InvalidFormat.ToString,
                                        "BatchReference")
                            ValidateFindReceiptDetails = False
                            Exit Function
                        End If
                    End If

                    If .AmountRangeFromSpecified AndAlso Len(Convert.ToString(.AmountRangeFrom).Trim) <> 0 Then
                        If IsNumeric(.AmountRangeFrom) <> True Then
                            oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidFormat,
                                            SAMInvalidData.InvalidFormat.ToString,
                                            "AmountRangeFrom")
                            ValidateFindReceiptDetails = False
                            Exit Function
                        End If
                    End If

                    If .AmountrangeToSpecified AndAlso Len(Convert.ToString(.AmountrangeTo).Trim) <> 0 Then
                        If IsNumeric(.AmountrangeTo) <> True Then
                            oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidFormat,
                                            SAMInvalidData.InvalidFormat.ToString,
                                            "AmountrangeTo")
                            ValidateFindReceiptDetails = False
                            Exit Function
                        End If
                    End If

                    If bIsCheque = True Then

                        If .MediaReferenceFrom IsNot Nothing AndAlso Len(Convert.ToString(.MediaReferenceFrom).Trim) <> 0 Then
                            If IsNumeric(.MediaReferenceFrom) <> True Or Len(Convert.ToString(.MediaReferenceFrom).Trim) > 10 Or Informations.inStr(Convert.ToString(.MediaReferenceFrom).Trim, ".") > 0 Then
                                oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidFormat,
                                                    SAMInvalidData.InvalidFormat.ToString,
                                                    "MediaReferenceFrom")
                                ValidateFindReceiptDetails = False
                                Exit Function
                            End If
                        End If

                        If .MediaReferenceTo IsNot Nothing AndAlso Len(Convert.ToString(.MediaReferenceTo).Trim) <> 0 Then
                            If IsNumeric(.MediaReferenceTo) <> True Or Len(Convert.ToString(.MediaReferenceTo).Trim) > 10 Or Informations.inStr(Convert.ToString(.MediaReferenceTo).Trim, ".") > 0 Then
                                oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidFormat,
                                                    SAMInvalidData.InvalidFormat.ToString,
                                                    "MediaReferenceTo")
                                ValidateFindReceiptDetails = False
                                Exit Function
                            End If
                        End If
                    Else
                        If .MediaReferenceFrom IsNot Nothing AndAlso Len(Convert.ToString(.MediaReferenceFrom).Trim) <> 0 Then
                            If ValidateString(Convert.ToString(.MediaReferenceFrom).Trim, "/\-_.") = False Then
                                oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidFormat,
                                                      SAMInvalidData.InvalidFormat.ToString,
                                                      "MediaReferenceFrom")
                                ValidateFindReceiptDetails = False
                                Exit Function
                            End If

                            If Informations.inStr(Convert.ToString(.MediaReferenceFrom).Trim, "/") = 1 Or Informations.inStr(Convert.ToString(.MediaReferenceFrom).Trim, "\") = 1 Or Informations.inStr(Convert.ToString(.MediaReferenceFrom).Trim, "-") = 1 Or Informations.inStr(Convert.ToString(.MediaReferenceFrom).Trim, "_") = 1 Or Informations.inStr(Convert.ToString(.MediaReferenceTo).Trim, ".") = 1 Then
                                oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidFormat,
                                                     SAMInvalidData.InvalidFormat.ToString,
                                                     "MediaReferenceFrom")
                                ValidateFindReceiptDetails = False
                                Exit Function
                            End If
                        End If
                        If .MediaReferenceTo IsNot Nothing AndAlso Len(Convert.ToString(.MediaReferenceTo).Trim) <> 0 Then
                            If ValidateString(Convert.ToString(.MediaReferenceTo).Trim, "/\-_.") = False Then
                                oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidFormat,
                                                   SAMInvalidData.InvalidFormat.ToString,
                                                   "MediaReferenceTo")
                                ValidateFindReceiptDetails = False
                                Exit Function
                            End If

                            If Informations.inStr(Convert.ToString(.MediaReferenceTo).Trim, "/") = 1 Or Informations.inStr(Convert.ToString(.MediaReferenceTo).Trim, "\") = 1 Or Informations.inStr(.MediaReferenceTo, "-") = 1 Or Informations.inStr(Convert.ToString(.MediaReferenceTo).Trim, "_") = 1 Or Informations.inStr(Convert.ToString(.MediaReferenceTo).Trim, ".") = 1 Then
                                oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidFormat,
                                                    SAMInvalidData.InvalidFormat.ToString,
                                                    "MediaReferenceTo")
                                ValidateFindReceiptDetails = False
                                Exit Function
                            End If

                            If Len(Convert.ToString(.PolicyClaimNumber).Trim) <> 0 Then
                                If ValidateString(Convert.ToString(.PolicyClaimNumber).Trim, "/\-_") = False Then
                                    oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidFormat,
                                                   SAMInvalidData.InvalidFormat.ToString,
                                                   "PolicyClaimNumber")
                                    ValidateFindReceiptDetails = False
                                    Exit Function
                                End If
                            End If
                        End If
                    End If

                    If .ReceiptBranch IsNot Nothing AndAlso String.IsNullOrEmpty(.ReceiptBranch) = False Then
                        iReceiptBranchId = oBusiness.GetAndValidateListItemFromCode(
                                                                            Core.STSListType.PMLookup,
                                                                            PMLookupTable.Source,
                                                                            .ReceiptBranch,
                                                                            "ReceiptBranch",
                                                                            oSAMErrorCollection)
                    End If
                    If .ReceiptStatus IsNot Nothing AndAlso String.IsNullOrEmpty(.ReceiptStatus) = False Then
                        iReceiptSatusID = oBusiness.GetAndValidateListItemFromCode(
                                                                            Core.STSListType.PMLookup,
                                                                            PMLookupTable.CashListItemReceiptStatus,
                                                                            .ReceiptStatus,
                                                                            "ReceiptStatus",
                                                                            oSAMErrorCollection)
                    End If
                    If .BankAccount IsNot Nothing AndAlso String.IsNullOrEmpty(.BankAccount) = False Then
                        iBankAccountID = oBusiness.GetAndValidateListItemFromCode(
                                                                            Core.STSListType.PMLookup,
                                                                            PMLookupTable.BankAccount,
                                                                            .BankAccount,
                                                                            "BankAccount",
                                                                            oSAMErrorCollection)
                    End If

                    If .MediaType IsNot Nothing AndAlso String.IsNullOrEmpty(.MediaType) = False Then
                        iMediatypeId = oBusiness.GetAndValidateListItemFromCode(
                                                                            Core.STSListType.PMLookup,
                                                                            PMLookupTable.MediaType,
                                                                            .MediaType,
                                                                            "MediaType",
                                                                            oSAMErrorCollection)
                    End If

                    oSAMErrorCollection.CheckForErrors()
                End With
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function

    ''' <summary>
    ''' CancelReceipt
    ''' </summary>
    ''' <param name="oRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CancelReceipt(ByVal oRequest As BaseCancelReceiptRequestType) As BaseCancelReceiptResponseType

        Dim oBusiness As New CoreBusiness
        Dim oResponse As New BaseImplementationTypes.BaseCancelReceiptResponseType
        Dim nTypeOfPackage As enumTypeOfPackage
        Dim oACTDocumentReversal As bACTDocumentReversal.Business = Nothing
        Dim oACTReceiptMaintenance As bACTPaymentMaintenance.Form = Nothing
        Dim iSourceId As Integer
        Dim iComReturnValue As Integer

        If oRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.CancelReceiptRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oResponse = New SAMForInsuranceV2ImplementationTypes.CancelReceiptResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
        End If

        Dim oSAMErrorCollection As New SAMErrorCollection

        ' Lookup Codes to ensure they are valid
        Try
            iSourceId = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.Source, oRequest.BranchCode, "BranchCode")
        Catch ex As Exception
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue,
            SAMInvalidData.InvalidLookupListValue.ToString,
            "BranchCode",
            oRequest.BranchCode)
        End Try
        oSAMErrorCollection.CheckForErrors()

        Try
            oRequest.ReverseReasonKey = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.CashListItemReverseReason, oRequest.ReverseReasonCode, "code")
        Catch ex As Exception
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue,
            SAMInvalidData.InvalidLookupListValue.ToString,
            "code",
            oRequest.BranchCode)
        End Try
        oSAMErrorCollection.CheckForErrors()

        '*******************
        ' DATA VALIDATION
        '*******************
        If oRequest.TransDetailKey = 0 Then
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
            SAMInvalidData.MandatoryInputMissing.ToString,
            "TransDetailKey")
        End If
        ' throw any data related errors
        oSAMErrorCollection.CheckForErrors()

        If oRequest.ReverseReasonKey = 0 Then
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
            SAMInvalidData.MandatoryInputMissing.ToString,
            "ReverseReasonKey")
        End If
        ' throw any data related errors
        oSAMErrorCollection.CheckForErrors()

        If oRequest.CashListItemKey = 0 Then
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
            SAMInvalidData.MandatoryInputMissing.ToString,
            "CashListItemKey")
        End If

        ' throw any data related errors
        oSAMErrorCollection.CheckForErrors()

        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)
            Try
                'oBusiness.CheckSAMTSAndLock(con, oRequest.BranchCode, _
                '                  CoreBusiness.LockName.TransDetailKey, _
                '                  oRequest.TransDetailKey, _
                '                  oRequest.TimeStamp)
                oACTDocumentReversal = New bACTDocumentReversal.Business
                SAMFunc.InitialiseSBOObject(con, oACTDocumentReversal, _SiriusUser, oRequest.BranchCode, "bACTDocumentReversal.Business")

                oACTDocumentReversal.TransDetailId = oRequest.TransDetailKey
                oACTDocumentReversal.IsCashlistItemReversal = True

               Dim sFailureReason As String = ""
                iComReturnValue = oACTDocumentReversal.Start(r_sFailureReason:=sFailureReason)

                If iComReturnValue <> 1 Then
                    If sFailureReason <> "" Then
                        oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.ReconcileError,
                                                           sFailureReason.ToString,
                                                            "The Selected cash item reversal failed.")
                    Else
                        oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.ReverseAllocationFailed,
                                                            SAMBusinessErrors.ReverseAllocationFailed.ToString,
                                                            "The Selected cash item reversal failed.")
                    End If
                    oSAMErrorCollection.CheckForErrors()
                Else
                    oACTReceiptMaintenance = New bACTPaymentMaintenance.Form
                    SAMFunc.InitialiseSBOObject(con, oACTReceiptMaintenance, _SiriusUser, oRequest.BranchCode, "bACTPaymentMaintenance.Form")

                    iComReturnValue = oACTReceiptMaintenance.SetCashListItemFlags(
                    v_lCashlistitem_id:=oRequest.CashListItemKey,
                    v_dtReversed_date:=Date.Now,
                    v_iCashlistitem_reverse_pmuser_id:=_SiriusUser.UserID,
                    v_lCashlistitem_reverse_reason_id:=oRequest.ReverseReasonKey,
                    v_lcashlistitem_reversal_transdetail_id:=oACTDocumentReversal.ReversalTransDetailId,
                    nIsReceiptReversal:=1)

                    If iComReturnValue <> PMEReturnCode.PMTrue AndAlso iComReturnValue <> PMEReturnCode.PMNotFound Then
                        RaiseComMethodException("bACTPaymentMaintenance.Form.SetCashListItemFlags", iComReturnValue)
                    End If
                End If

                Return oResponse

            Catch ex As Exception
                Throw

            Finally
                If oACTDocumentReversal IsNot Nothing Then
                    oACTDocumentReversal.Dispose()
                    oACTDocumentReversal = Nothing
                End If
                If oACTReceiptMaintenance IsNot Nothing Then
                    oACTReceiptMaintenance.Dispose()
                    oACTReceiptMaintenance = Nothing
                End If
                'oBusiness.UnlockAndGetSAMTS(con, _
                '                     oRequest.BranchCode, _
                '                     CoreBusiness.LockName.TransDetailKey, _
                '                     oRequest.TransDetailKey, _
                '                     oResponse.TimeStamp)
            End Try
        End Using
    End Function

#End Region

#Region "Receipt/Payment Document"



    Private Sub GenerateCashChequeReceiptAndPaymentDocument(
       ByVal con As SiriusConnection, ByVal v_sBranchCode As String, ByVal v_nSourceId As Integer, ByVal v_nPartyKey As Integer, ByVal v_sType As String, ByVal v_sDocumentRef As String)
        GenerateCashChequeReceiptAndPaymentDocument(con:=con, v_sBranchCode:=v_sBranchCode, v_nSourceId:=v_nSourceId, v_nPartyKey:=v_nPartyKey,
                                                   v_sType:=v_sType, v_sDocumentRef:=v_sDocumentRef, v_sDocumentCode:=Nothing, v_nInsuranceFileKey:=0, v_nInsuranceFolderKey:=0)
    End Sub

    Private Sub GenerateCashChequeReceiptAndPaymentDocument(
       ByVal con As SiriusConnection, ByVal v_sBranchCode As String, ByVal v_nSourceId As Integer, ByVal v_nPartyKey As Integer, ByVal v_sType As String, ByVal v_sDocumentRef As String, ByRef v_sDocumentCode As String)
        GenerateCashChequeReceiptAndPaymentDocument(con:=con, v_sBranchCode:=v_sBranchCode, v_nSourceId:=v_nSourceId, v_nPartyKey:=v_nPartyKey,
                                                   v_sType:=v_sType, v_sDocumentRef:=v_sDocumentRef, v_sDocumentCode:=v_sDocumentCode, v_nInsuranceFileKey:=0, v_nInsuranceFolderKey:=0)
    End Sub



    Private Sub GenerateCashChequeReceiptAndPaymentDocument(
       ByVal con As SiriusConnection, ByVal v_sBranchCode As String, ByVal v_nSourceId As Integer, ByVal v_nPartyKey As Integer, ByVal v_sType As String, ByVal v_sDocumentRef As String, ByRef v_sDocumentCode As String, ByVal v_nInsuranceFileKey As Integer, ByVal v_nInsuranceFolderKey As Integer)

        Dim generateDocumentRequest As BaseGenerateDocumentRequestType = New BaseGenerateDocumentRequestType
        Dim generateDocumentResponse As BaseGenerateDocumentResponseType

        Dim nDocumentTemplateId As Integer = 0
        Dim oBusiness As New CoreBusiness
        Dim sDefaultDocument As String = ""

        If v_sType = "R" Then
            oBusiness.GetSystemOption(v_sBranchCode, SystemOption.DefaultReceiptDocument, sDefaultDocument)
        ElseIf v_sType = "P" Then
            oBusiness.GetSystemOption(v_sBranchCode, SystemOption.DefaultPaymentDocument, sDefaultDocument)
        End If

        If Not (Trim(sDefaultDocument) = "0") And Trim(sDefaultDocument) <> String.Empty Then
            nDocumentTemplateId = CType(sDefaultDocument, Integer)
        End If

        If (nDocumentTemplateId > 0) Then
            v_sDocumentCode = GetTemplateCode(con, nDocumentTemplateId)
            generateDocumentRequest.DocumentTemplateCode = v_sDocumentCode
            generateDocumentRequest.InsuranceFolderKey = v_nInsuranceFolderKey
            generateDocumentRequest.InsuranceFileKey = v_nInsuranceFileKey
            generateDocumentRequest.BranchCode = v_sBranchCode
            generateDocumentRequest.Mode = 4
            generateDocumentRequest.OutputAsHTML = False
            generateDocumentRequest.OutputAsPDF = False
            generateDocumentRequest.PartyKey = v_nPartyKey
            generateDocumentRequest.SourceId = v_nSourceId
            generateDocumentRequest.SpoolDocumentOnly = False
            generateDocumentRequest.DocumentRef = v_sDocumentRef
            generateDocumentRequest.ArchiveDocFileName = v_sDocumentRef.Trim()

            generateDocumentResponse = GenerateDocument(generateDocumentRequest, con)

            ' ensure any sts errors raised by this legacy function are handled correctly
            SAMErrorCollection.CheckForErrorsFromSTS(generateDocumentResponse.STSError)

        End If

    End Sub

    Private Function GetCurrencyDiff(ByVal con As SiriusConnection, ByVal oCashListItem() As BaseCreatePaymentCashListWithItemsResponseTypeCashListItem, ByVal dCurrencyExRate As Double) As Double
        Dim dCurrencyDiff As Double = 0
        Dim iTransactionKey As Integer = 0
        Dim sAccountShortCode As String = ""
        Dim ds As DataSet = Nothing

        For Each CashListItem As BaseCreatePaymentCashListWithItemsResponseTypeCashListItem In oCashListItem
            iTransactionKey = CashListItem.TransDetailKey
            sAccountShortCode = CashListItem.AccountShortCode
            If ToSafeInteger(iTransactionKey) <> 0 AndAlso ToSafeString(sAccountShortCode) <> "" Then
                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_GetCurrencyDifference")

                    cmd.AddInParameter("@TransDetailId", SqlDbType.Int).Value = iTransactionKey
                    cmd.AddInParameter("@AccountShortCode", SqlDbType.VarChar, 255).Value = sAccountShortCode
                    cmd.AddInParameter("@OldCurrencyRate", SqlDbType.Decimal).Value = dCurrencyExRate

                    ds = con.ExecuteDataSet(cmd, "CurrencyDifference")

                End Using

                If ds.Tables("CurrencyDifference").Rows.Count > 0 Then
                    dCurrencyDiff = dCurrencyDiff + Double.Parse(ds.Tables("CurrencyDifference").Rows(0)(0).ToString())
                End If
            End If
        Next
        Return dCurrencyDiff
    End Function

#End Region

    ''' <summary>
    ''' Get the other allocation Trans from the database 
    ''' with WriteOff and CurrencyGainLoss amount as well
    '''</summary>
    '''<param name="con" >This is an object of a class SiriusConnection </param>   
    '''<param name="oAllocation">This is an object of a class BaseAllocationType</param>  
    '''<param name="o_bProceedWithAutoAllocation"></param>      
    '''<param name="o_crWriteOffAmount"></param>      
    '''<param name="o_crCurrencyGainLossAutoAllocationLimitAmount"></param>              
    '''<remarks></remarks>
    Private Sub CheckWriteOffAndExchangeRateGainLoss(ByVal con As SiriusConnection,
                                                ByRef o_oAllocation As BaseAllocationType,
                                                ByRef o_bProceedWithAutoAllocation As Boolean,
                                                ByRef o_crWriteOffAmount As Decimal,
                                                ByRef o_crCurrencyGainLossAutoAllocationLimitAmount As Decimal)
        Dim dsTransactions As New DataSet
        Dim nCount As Integer = 0
        Dim crAccountBalance As Decimal
        Dim oCoreBusiness As New CoreBusiness(_SiriusUser)

        o_bProceedWithAutoAllocation = True

        If (o_oAllocation IsNot Nothing) Then
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_ACT_Select_trans_for_allocation")
                cmd.AddInParameter("@account_id", SqlDbType.Int).Value = Cast.NullIfDefault(o_oAllocation.AccountKey)
                cmd.AddInParameter("@company_id", SqlDbType.Int).Value = Cast.NullIfDefault(o_oAllocation.SourceID)
                cmd.AddInParameter("@DNGIND", SqlDbType.Int).Value = 0
                cmd.AddOutParameter("@base_balance", SqlDbType.Money)
                dsTransactions = con.ExecuteDataSet(cmd, "PayClaim")

                crAccountBalance = gPMFunctions.ToSafeDecimal(cmd.Parameters("@base_balance").Value, CDec(0))
            End Using

            Dim oDocumentTypeTypeList As New List(Of String)
            oDocumentTypeTypeList.Add(DocumentTypeType.IEC.ToString)
            oDocumentTypeTypeList.Add(DocumentTypeType.IED.ToString)
            oDocumentTypeTypeList.Add(DocumentTypeType.IIC.ToString)
            oDocumentTypeTypeList.Add(DocumentTypeType.IID.ToString)
            oDocumentTypeTypeList.Add(DocumentTypeType.INC.ToString)
            oDocumentTypeTypeList.Add(DocumentTypeType.IND.ToString)
            oDocumentTypeTypeList.Add(DocumentTypeType.IRC.ToString)
            oDocumentTypeTypeList.Add(DocumentTypeType.IRD.ToString)

            If dsTransactions IsNot Nothing AndAlso dsTransactions.Tables.Count > 0 AndAlso dsTransactions.Tables(0).Rows.Count > 0 Then
                ReDim o_oAllocation.OtherAllocatingTrans(dsTransactions.Tables(0).Rows.Count - 1)
                For nCount = 0 To dsTransactions.Tables(0).Rows.Count - 1
                    o_oAllocation.OtherAllocatingTrans(nCount) = New BaseTransDetailType
                    o_oAllocation.OtherAllocatingTrans(nCount).TransDetailKey = Cast.ToInt32 _
                    (dsTransactions.Tables(0).Rows(nCount).Item("transdetail_id"), 0)
                    o_oAllocation.OtherAllocatingTrans(nCount).Amount = Cast.ToDecimal _
                    (dsTransactions.Tables(0).Rows(nCount).Item("outstanding_amount"), 0)
                    o_oAllocation.OtherAllocatingTrans(nCount).AmountCurrencyId = Cast.ToInt32(dsTransactions.Tables(0).Rows(nCount).Item("amount_currency_id"), 0)
                    'In case of outstanding Instalment Type Transactions
                    If oDocumentTypeTypeList.Contains _
                    (Cast.ToString(dsTransactions.Tables(0).Rows(nCount).Item("document_ref"), "").Substring(0, 3)) Then
                        o_bProceedWithAutoAllocation = False
                        Return
                    End If

                Next
            Else
                o_bProceedWithAutoAllocation = False
                Return
            End If
        End If

        Dim obACTCashListItem As New bACTCashlistitem.Form
        Dim nComReturnValue As Integer = 0
        Dim oDatabase As Object = Nothing
        If Not con Is Nothing Then
            oDatabase = Nothing
            oDatabase = con.PMDAODatabase
        End If
        nComReturnValue = obACTCashListItem.Initialise(_SiriusUser.Username,
                                               _SiriusUser.Password,
                                               CShort(_SiriusUser.UserID),
                                               CShort(_SiriusUser.SourceID),
                                               CShort(_SiriusUser.LanguageID),
                                               CShort(_SiriusUser.CurrencyID),
                                               1, SiriusUserDefaults.AppName,
                                               vDatabase:=oDatabase)
        If nComReturnValue <> PMEReturnCode.PMTrue Then
            RaiseComMethodException("bACTCashlistitem.Initialise", nComReturnValue)
        End If

        Dim nBranchBaseCurrencyID As Integer
        nComReturnValue = obACTCashListItem.GetBranchBaseCurrency(o_oAllocation.SourceID, nBranchBaseCurrencyID)
        If nComReturnValue <> PMEReturnCode.PMTrue Then
            RaiseComMethodException("bACTCashlistitem.Form.GetBranchBaseCurrency", nComReturnValue)
        End If

        Dim nCurrencyID As Integer
        Dim dsPaymentCashListItem As DataSet
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_ACT_Select_CashListItem")
            cmd.AddInParameter("@cashlistitem_id", SqlDbType.Int).Value = o_oAllocation.LeadAllocatingTrans.CashListItemKey
            dsPaymentCashListItem = con.ExecuteDataSet(cmd, "CashListPayment")
        End Using


        If dsPaymentCashListItem IsNot Nothing _
        AndAlso dsPaymentCashListItem.Tables.Count > 0 _
        AndAlso dsPaymentCashListItem.Tables(0).Rows.Count > 0 Then
            nCurrencyID = Cast.ToInt32(dsPaymentCashListItem.Tables(0).Rows(0).Item("currency_id"), 0)
            If o_oAllocation.LeadAllocatingTrans.TransDetailKey = 0 Then
                o_oAllocation.LeadAllocatingTrans.TransDetailKey =
                Cast.ToInt32(dsPaymentCashListItem.Tables(0).Rows(0).Item("transdetail_id"), 0)
            End If
        Else
            RaiseComMethodException("Failed to get Curency ID spu_ACT_Select_CashListItem", 0)
        End If

        If crAccountBalance = CDec(0) Then
            o_bProceedWithAutoAllocation = True
            Return
        End If

        If nBranchBaseCurrencyID = nCurrencyID Then

            Dim dsUserAuthority As DataSet
            Dim oRequest As New BaseGetUserAuthorityValueRequestType
            Dim bAllocationWriteOffAuthority As Boolean = False
            Dim crAllocationWriteOffAuthorityAmount As Decimal

            oRequest.UserAuthorityOption = UserAuthorityOptions.HasWriteOffAuthority
            dsUserAuthority = GetValueForUserAuthority(con, _SiriusUser.UserID, oRequest)

            If Not dsUserAuthority Is Nothing AndAlso
            dsUserAuthority.Tables.Count > 0 AndAlso
            dsUserAuthority.Tables(0).Rows.Count > 0 Then
                If dsUserAuthority.Tables(0).Rows(0).Item(0).ToString() = "1" Then
                    bAllocationWriteOffAuthority = True
                    crAllocationWriteOffAuthorityAmount = gPMFunctions.ToSafeDecimal(dsUserAuthority.Tables(0).Rows(0).Item(2).ToString())
                Else
                    o_bProceedWithAutoAllocation = False
                    Return
                End If
            Else
                o_bProceedWithAutoAllocation = False
                Return
            End If

            If Math.Abs(crAccountBalance) <= crAllocationWriteOffAuthorityAmount Then
                o_crWriteOffAmount = crAccountBalance
                o_crCurrencyGainLossAutoAllocationLimitAmount = 0
                Return
            Else
                o_bProceedWithAutoAllocation = False
                Return
            End If

        Else
            ''When transaction is other than base currency
            Dim sOptionValue As String = String.Empty
            Const kCurGainLossAllocationLimit As Integer = 5060
            oCoreBusiness.GetSystemOption(o_oAllocation.BranchCode, kCurGainLossAllocationLimit, sOptionValue)

            If gPMFunctions.ToSafeDouble(sOptionValue) = 0 Then
                Return
            End If

            If crAccountBalance <> CDec(0) Then
                If Math.Abs(crAccountBalance) <=
                Math.Abs((o_oAllocation.LeadAllocatingTrans.Amount * gPMFunctions.ToSafeDecimal(sOptionValue)) / 100) Then

                    o_crCurrencyGainLossAutoAllocationLimitAmount = crAccountBalance
                    o_crWriteOffAmount = 0
                End If
            End If
        End If

    End Sub

    '''<summary>
    '''This method sends the request object values to the Stored Procedure and COM to Approve or decline the transaction Selected columns 
    '''</summary>
    '''<param name="oBaseGetUserPreferredColumnListRequestType" type="BaseGetUserPreferredColumnListRequestType"></param>
    ''' <returns>BaseGetUserPreferredColumnListResponseType</returns>
    '''<remarks></remarks>
    Public Overloads Function GetUserPreferredColumnList(ByVal oBaseGetUserPreferredColumnListRequestType As BaseGetUserPreferredColumnListRequestType) As BaseGetUserPreferredColumnListResponseType

        Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
            Dim oBaseGetUserPreferredColumnResponseType As BaseGetUserPreferredColumnListResponseType
            oBaseGetUserPreferredColumnResponseType = GetUserPreferredColumnList(oBaseGetUserPreferredColumnListRequestType, con)
            Return oBaseGetUserPreferredColumnResponseType
        End Using
    End Function

    '''<summary>
    '''This method sends the request object values to the Stored Procedure and COM to Get the transaction Selected columns 
    '''</summary>
    '''<param name="oBaseGetUserPreferredColumnRequestType" type="oBaseGetUserPreferredColumnRequestType"></param>
    ''' <returns>BaseGetUserPreferredColumnListResponseType</returns>
    '''<remarks></remarks>
    Public Overloads Function GetUserPreferredColumnList(ByVal oBaseGetUserPreferredColumnRequestType As BaseGetUserPreferredColumnListRequestType, ByVal con As SiriusConnection) As BaseGetUserPreferredColumnListResponseType
        Try
            Dim oErrors As New SAMErrorCollection
            Dim dsSelectedColumn As New DataSet
            Dim oBaseGetUserPreferredColumnResponseType As New BaseGetUserPreferredColumnListResponseType

            oBaseGetUserPreferredColumnRequestType.Validate(CObj(oErrors))
            oErrors.CheckForErrors()
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_Get_User_Preferred_Column_List")
                cmd.AddInParameter("@sUserName", SqlDbType.VarChar).Value = oBaseGetUserPreferredColumnRequestType.LoginUserName
                cmd.AddInParameter("@sInterfaceName", SqlDbType.VarChar).Value = oBaseGetUserPreferredColumnRequestType.InterfaceName
                dsSelectedColumn = con.ExecuteDataSet(cmd, "User_Preferred_Column_List")
            End Using
            If dsSelectedColumn IsNot Nothing AndAlso dsSelectedColumn.Tables.Count > 0 AndAlso dsSelectedColumn.Tables(0).Rows.Count > 0 Then
                With oBaseGetUserPreferredColumnResponseType
                    '.UserName = Convert.ToString(dsSelectedColumn.Tables(0).Rows(0).Item("UserName"))
                    '.InterfaceName = Convert.ToString(dsSelectedColumn.Tables(0).Rows(0).Item("InterfaceName"))
                    .ColumnList = Convert.ToString(dsSelectedColumn.Tables(0).Rows(0).Item("ColumnList"))
                End With
            End If
            Return oBaseGetUserPreferredColumnResponseType
        Catch ex As Exception
            con.RollbackTransaction()
            Throw
        End Try
    End Function

    ''' <summary>
    ''' This method sends the request object values to the Stored Procedure and COM to Update the transaction Selected columns 
    ''' </summary>
    ''' <param name="oBaseUpdateUserPreferredColumnListRequestType"></param>
    ''' <returns></returns>
    Public Overloads Function UpdateUserPreferredColumnList(ByVal oBaseUpdateUserPreferredColumnListRequestType As BaseUpdateUserPreferredColumnListRequestType) As BaseUpdateUserPreferredColumnListResponseType
        Dim oBaseUpdateUserPreferredColumnListResponseType As New BaseUpdateUserPreferredColumnListResponseType
        Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)

            oBaseUpdateUserPreferredColumnListResponseType = UpdateUserPreferredColumnList(oBaseUpdateUserPreferredColumnListRequestType, con)
            Return oBaseUpdateUserPreferredColumnListResponseType
        End Using
    End Function

    ''' <summary>
    ''' This method sends the request object values to the Stored Procedure and COM to Update the transaction Selected columns 
    ''' </summary>
    ''' <param name="oBaseUpdateUserPreferredColumnListRequestType"></param>
    ''' <param name="con"></param>
    ''' <returns></returns>
    Public Overloads Function UpdateUserPreferredColumnList(ByVal oBaseUpdateUserPreferredColumnListRequestType As BaseUpdateUserPreferredColumnListRequestType, ByVal con As SiriusConnection) As BaseUpdateUserPreferredColumnListResponseType
        Try
            Dim oErrors As New SAMErrorCollection
            Dim oResponse As New BaseUpdateUserPreferredColumnListResponseType
            oBaseUpdateUserPreferredColumnListRequestType.Validate(CObj(oErrors))
            oErrors.CheckForErrors()
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_Update_User_Preferred_Column_List")

                cmd.AddInParameter("@sUserName", SqlDbType.VarChar).Value = oBaseUpdateUserPreferredColumnListRequestType.LoginUserName
                cmd.AddInParameter("@sInterfaceName", SqlDbType.VarChar).Value = oBaseUpdateUserPreferredColumnListRequestType.InterfaceName
                cmd.AddInParameter("@sColumnList", SqlDbType.VarChar).Value = oBaseUpdateUserPreferredColumnListRequestType.ColumnList
                con.ExecuteDataSet(cmd, "User_Preferred_Column_List")
            End Using
            Return oResponse
        Catch ex As Exception
            con.RollbackTransaction()
            Throw
        End Try
    End Function

    ''' <summary>
    ''' Get list of unapproved payments.
    ''' </summary>
    ''' <param name="oBasePaymentAuthorisationRequestType"></param>
    ''' <returns></returns>
    Public Overloads Function GetListofUnapprovedPayment(ByVal oBasePaymentAuthorisationRequestType As BaseGetListofUnapprovedPaymentRequestType) As BaseGetListofUnapprovedPaymentResponseType
        Dim oBaseGetListofUnapprovedPaymentResponseType As New BaseGetListofUnapprovedPaymentResponseType
        Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)

            oBaseGetListofUnapprovedPaymentResponseType = GetListofUnapprovedPayment(oBasePaymentAuthorisationRequestType, con)
            Return oBaseGetListofUnapprovedPaymentResponseType
        End Using
    End Function

    Public Overloads Function GetListofUnapprovedPayment(ByVal oBaseGetListofUnapprovedPaymentRequestType As BaseGetListofUnapprovedPaymentRequestType, ByVal con As SiriusConnection) As BaseGetListofUnapprovedPaymentResponseType
        Try
            Dim oErrors As New SAMErrorCollection
            Dim dsCashList As New DataSet
            Dim dsResultantDataSet As New DataSet
            ' Dim oResponse As New BaseUpdateSearchTransactionSelectedColumnResponseType
            Dim oBaseGetListofUnapprovedPaymentResponseType As BaseGetListofUnapprovedPaymentResponseType
            oBaseGetListofUnapprovedPaymentRequestType.Validate(CObj(oErrors))
            'oErrors.CheckForErrors()
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_ACT_Get_CashList_For_Authorization")
                cmd.AddInParameter("@AssignedTo", SqlDbType.VarChar).Value = oBaseGetListofUnapprovedPaymentRequestType.AssignedTo
                cmd.AddInParameter("@CashListItemId", SqlDbType.VarChar).Value = oBaseGetListofUnapprovedPaymentRequestType.CashListItemKey
                cmd.AddInParameter("@Branch", SqlDbType.VarChar).Value = oBaseGetListofUnapprovedPaymentRequestType.Branch
                cmd.AddInParameter("@CreatedBy", SqlDbType.VarChar).Value = oBaseGetListofUnapprovedPaymentRequestType.CreatedBy
                If (oBaseGetListofUnapprovedPaymentRequestType.DateFrom <> DateTime.MinValue) Then
                    cmd.AddInParameter("@Date_From", SqlDbType.DateTime).Value = oBaseGetListofUnapprovedPaymentRequestType.DateFrom
                End If
                If (oBaseGetListofUnapprovedPaymentRequestType.DateTo <> DateTime.MinValue) Then
                    cmd.AddInParameter("@Date_To", SqlDbType.DateTime).Value = oBaseGetListofUnapprovedPaymentRequestType.DateTo
                End If
                cmd.AddInParameter("@PayeeName", SqlDbType.VarChar).Value = oBaseGetListofUnapprovedPaymentRequestType.PayeeName
                If Not String.IsNullOrEmpty(oBaseGetListofUnapprovedPaymentRequestType.PaymentType) Then
                    cmd.AddInParameter("@PaymentType", SqlDbType.VarChar).Value = oBaseGetListofUnapprovedPaymentRequestType.PaymentType
                End If
                dsCashList = con.ExecuteDataSet(cmd, "Row")
            End Using


            If (dsCashList.Tables.Count >= 1) AndAlso (dsCashList.Tables(0).Rows.Count >= 1) Then
                With dsCashList.Tables(0)
                    dsResultantDataSet.Tables.Add()
                    dsResultantDataSet.Tables(0).Columns.Add("Branch", GetType(System.String))
                    dsResultantDataSet.Tables(0).Columns.Add("Currency", GetType(System.String))
                    dsResultantDataSet.Tables(0).Columns.Add("BankAccount", GetType(System.String))
                    dsResultantDataSet.Tables(0).Columns.Add("TransactionDate", GetType(System.DateTime))
                    dsResultantDataSet.Tables(0).Columns.Add("PaymentType", GetType(System.String))
                    dsResultantDataSet.Tables(0).Columns.Add("MediaType", GetType(System.String))
                    dsResultantDataSet.Tables(0).Columns.Add("MediaRef", GetType(System.String))
                    dsResultantDataSet.Tables(0).Columns.Add("ClaimRef", GetType(System.String))
                    dsResultantDataSet.Tables(0).Columns.Add("PayeeAccountName", GetType(System.String))
                    dsResultantDataSet.Tables(0).Columns.Add("Amount", GetType(System.Decimal))
                    dsResultantDataSet.Tables(0).Columns.Add("CreatedBy", GetType(System.String))
                    dsResultantDataSet.Tables(0).Columns.Add("Status", GetType(System.String))
                    dsResultantDataSet.Tables(0).Columns.Add("Assignedto", GetType(System.String))
                    dsResultantDataSet.Tables(0).Columns.Add("DateAssigned", GetType(System.DateTime))
                    dsResultantDataSet.Tables(0).Columns.Add("BaseCurrencyAmount", GetType(System.Decimal))
                    dsResultantDataSet.Tables(0).Columns.Add("PolicyRef", GetType(System.String))
                    dsResultantDataSet.Tables(0).Columns.Add("CashListId", GetType(System.Int32))
                    dsResultantDataSet.Tables(0).Columns.Add("CashListItemId", GetType(System.Int32))

                    For icount = 0 To .Rows.Count - 1
                        dsResultantDataSet.Tables(0).Rows.Add()
                        dsResultantDataSet.Tables(0).Rows(icount)("Branch") = .Rows(icount).Item("Branch")
                        dsResultantDataSet.Tables(0).Rows(icount)("Currency") = .Rows(icount).Item("CurrencyCode")
                        dsResultantDataSet.Tables(0).Rows(icount)("BankAccount") = .Rows(icount).Item("Bank_Account")
                        dsResultantDataSet.Tables(0).Rows(icount)("TransactionDate") = .Rows(icount).Item("transaction_date")
                        dsResultantDataSet.Tables(0).Rows(icount)("PaymentType") = .Rows(icount).Item("Payment_type")
                        dsResultantDataSet.Tables(0).Rows(icount)("MediaType") = .Rows(icount).Item("media_type")
                        dsResultantDataSet.Tables(0).Rows(icount)("MediaRef") = .Rows(icount).Item("media_ref")
                        dsResultantDataSet.Tables(0).Rows(icount)("ClaimRef") = .Rows(icount).Item("claim_ref")
                        dsResultantDataSet.Tables(0).Rows(icount)("PayeeAccountName") = .Rows(icount).Item("account_name")
                        dsResultantDataSet.Tables(0).Rows(icount)("Amount") = .Rows(icount).Item("amount")
                        dsResultantDataSet.Tables(0).Rows(icount)("CreatedBy") = .Rows(icount).Item("username")
                        dsResultantDataSet.Tables(0).Rows(icount)("Status") = .Rows(icount).Item("CurrentStatus")
                        dsResultantDataSet.Tables(0).Rows(icount)("Assignedto") = .Rows(icount).Item("Assigned_to")
                        dsResultantDataSet.Tables(0).Rows(icount)("DateAssigned") = .Rows(icount).Item("Assigned_date")
                        dsResultantDataSet.Tables(0).Rows(icount)("BaseCurrencyAmount") = .Rows(icount).Item("base_currency_amount")
                        dsResultantDataSet.Tables(0).Rows(icount)("PolicyRef") = .Rows(icount).Item("Policy_Ref")
                        dsResultantDataSet.Tables(0).Rows(icount)("CashListId") = .Rows(icount).Item("cashlist_id")
                        dsResultantDataSet.Tables(0).Rows(icount)("CashListItemId") = .Rows(icount).Item("cashListItem_id")
                    Next
                End With
            End If

            oBaseGetListofUnapprovedPaymentResponseType = New BaseGetListofUnapprovedPaymentResponseType
            Dim Docxml As New System.Xml.XmlDocument
            dsResultantDataSet.DataSetName = "BaseGetListofUnapprovedPaymentRequestType"
            If (dsResultantDataSet.Tables.Count >= 1) Then
                oBaseGetListofUnapprovedPaymentResponseType.ResultDataset = dsResultantDataSet
            End If
            Return oBaseGetListofUnapprovedPaymentResponseType
        Catch ex As Exception
            con.RollbackTransaction()
            Throw
        End Try

    End Function

    ''' <summary>
    ''' Get list of unapproved Manual Journal Transactions.
    ''' </summary>
    ''' <param name="oBaseManualJournalAuthorisationRequestType"></param>
    ''' <returns></returns>
    Public Overloads Function GetListofManualJournalTransactions(ByVal oBaseManualJournalAuthorisationRequestType As BaseGetListofManualJournalTransactionsRequestType) As BaseGetListofManualJournalTransactionsResponseType
        Dim oBaseGetListofManualJournalTransactionsResponseType As New BaseGetListofManualJournalTransactionsResponseType
        Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)

            oBaseGetListofManualJournalTransactionsResponseType = GetListofManualJournalTransactions(oBaseManualJournalAuthorisationRequestType, con)
            Return oBaseGetListofManualJournalTransactionsResponseType
        End Using
    End Function

    Public Overloads Function GetListofManualJournalTransactions(ByVal oBaseManualJournalAuthorisationRequestType As BaseGetListofManualJournalTransactionsRequestType, ByVal con As SiriusConnection) As BaseGetListofManualJournalTransactionsResponseType
        Try
            Dim oCoreBusiness As New CoreBusiness
            Dim oErrors As New SAMErrorCollection
            Dim dsManualJournalTrans As New DataSet
            Dim dsResultantDataSet As New DataSet
            Dim iDocumentTypeId As Integer
            iDocumentTypeId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.DocumentTypes, oBaseManualJournalAuthorisationRequestType.JournalTypeCode, "JournalTypeCode", oErrors)

            Dim oBaseGetListofManualJournalTransactionsResponseType As BaseGetListofManualJournalTransactionsResponseType
            oBaseManualJournalAuthorisationRequestType.Validate(CObj(oErrors))
            'oErrors.CheckForErrors()
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_Get_UnAuthorised_Manual_Journals")
                cmd.AddInParameter("@accountCode", SqlDbType.VarChar).Value = Cast.NullIfDefault(oBaseManualJournalAuthorisationRequestType.AccountCode, String.Empty)
                cmd.AddInParameter("@documentTypeId", SqlDbType.Int).Value = iDocumentTypeId

                If (oBaseManualJournalAuthorisationRequestType.DateFrom <> DateTime.MinValue) Then
                    cmd.AddInParameter("@dateFrom", SqlDbType.DateTime).Value = oBaseManualJournalAuthorisationRequestType.DateFrom
                End If
                If (oBaseManualJournalAuthorisationRequestType.DateTo <> DateTime.MinValue) Then
                    cmd.AddInParameter("@dateTo", SqlDbType.DateTime).Value = oBaseManualJournalAuthorisationRequestType.DateTo
                End If
                dsManualJournalTrans = con.ExecuteDataSet(cmd, "Row")
            End Using


            If (dsManualJournalTrans.Tables.Count >= 1) AndAlso (dsManualJournalTrans.Tables(0).Rows.Count >= 1) Then
                With dsManualJournalTrans.Tables(0)
                    dsResultantDataSet.Tables.Add()
                    dsResultantDataSet.Tables(0).Columns.Add("AccountCode", GetType(System.String))
                    dsResultantDataSet.Tables(0).Columns.Add("Amount", GetType(System.Decimal))
                    dsResultantDataSet.Tables(0).Columns.Add("Currency", GetType(System.String))
                    dsResultantDataSet.Tables(0).Columns.Add("CurrencyRate", GetType(System.Decimal))
                    dsResultantDataSet.Tables(0).Columns.Add("BaseCurrencyAmount", GetType(System.Decimal))
                    dsResultantDataSet.Tables(0).Columns.Add("AlternateRef", GetType(System.String))
                    dsResultantDataSet.Tables(0).Columns.Add("Comment", GetType(System.String))
                    dsResultantDataSet.Tables(0).Columns.Add("UnderwritingYearId", GetType(System.String))
                    dsResultantDataSet.Tables(0).Columns.Add("CostCenterId", GetType(System.String))
                    dsResultantDataSet.Tables(0).Columns.Add("InsuranceRef", GetType(System.String))
                    dsResultantDataSet.Tables(0).Columns.Add("PurchaseOrderNumber", GetType(System.String))
                    dsResultantDataSet.Tables(0).Columns.Add("PurchaseInvoiceNumber", GetType(System.String))
                    dsResultantDataSet.Tables(0).Columns.Add("ManualJournalId", GetType(System.Int32))
                    dsResultantDataSet.Tables(0).Columns.Add("CurrencyCode", GetType(System.String))
                    dsResultantDataSet.Tables(0).Columns.Add("CreatedBy", GetType(System.String))
                    dsResultantDataSet.Tables(0).Columns.Add("CreatedDate", GetType(System.DateTime))
                    dsResultantDataSet.Tables(0).Columns.Add("Status", GetType(System.String))
                    For icount = 0 To .Rows.Count - 1
                        dsResultantDataSet.Tables(0).Rows.Add()
                        dsResultantDataSet.Tables(0).Rows(icount)("AccountCode") = .Rows(icount).Item("AccountCode")
                        dsResultantDataSet.Tables(0).Rows(icount)("Amount") = .Rows(icount).Item("Amount")
                        dsResultantDataSet.Tables(0).Rows(icount)("Currency") = .Rows(icount).Item("Currency_Id")
                        dsResultantDataSet.Tables(0).Rows(icount)("CurrencyRate") = .Rows(icount).Item("Currency_Rate")
                        dsResultantDataSet.Tables(0).Rows(icount)("BaseCurrencyAmount") = .Rows(icount).Item("Base_Amount")
                        dsResultantDataSet.Tables(0).Rows(icount)("AlternateRef") = .Rows(icount).Item("Alternate_Ref")
                        dsResultantDataSet.Tables(0).Rows(icount)("Comment") = .Rows(icount).Item("Comment")
                        dsResultantDataSet.Tables(0).Rows(icount)("UnderwritingYearId") = .Rows(icount).Item("UnderwritingYear_Id")
                        dsResultantDataSet.Tables(0).Rows(icount)("CostCenterId") = .Rows(icount).Item("CostCenterId")
                        dsResultantDataSet.Tables(0).Rows(icount)("InsuranceRef") = .Rows(icount).Item("Insurance_Ref")
                        dsResultantDataSet.Tables(0).Rows(icount)("PurchaseOrderNumber") = .Rows(icount).Item("Purchase_Order_No")
                        dsResultantDataSet.Tables(0).Rows(icount)("PurchaseInvoiceNumber") = .Rows(icount).Item("Purchase_Invoice_No")
                        dsResultantDataSet.Tables(0).Rows(icount)("ManualJournalId") = .Rows(icount).Item("ManualJournal_Id")
                        dsResultantDataSet.Tables(0).Rows(icount)("CurrencyCode") = .Rows(icount).Item("CurrencyCode")
                        dsResultantDataSet.Tables(0).Rows(icount)("CreatedBy") = .Rows(icount).Item("CreatedBy").ToString.Trim()
                        dsResultantDataSet.Tables(0).Rows(icount)("CreatedDate") = .Rows(icount).Item("CreatedDate").ToString.Trim()
                        dsResultantDataSet.Tables(0).Rows(icount)("Status") = .Rows(icount).Item("Status").ToString.Trim()
                    Next
                End With
            End If

            oBaseGetListofManualJournalTransactionsResponseType = New BaseGetListofManualJournalTransactionsResponseType
            Dim Docxml As New System.Xml.XmlDocument
            dsResultantDataSet.DataSetName = "BaseGetListofManualJournalTransactionsRequestType"
            If (dsResultantDataSet.Tables.Count >= 1) Then
                oBaseGetListofManualJournalTransactionsResponseType.ResultDataset = dsResultantDataSet
            End If
            Return oBaseGetListofManualJournalTransactionsResponseType
        Catch ex As Exception
            con.RollbackTransaction()
            Throw
        End Try

    End Function

    ''' <summary>
    ''' This method is used to update comment in the cashListItem table for payment authorization
    ''' </summary>
    ''' <param name="oBaseUpdateAuthorizationCommentRequestType"></param>
    ''' <returns></returns>
    Public Overloads Function UpdateAuthorizationComment(ByVal oBaseUpdateAuthorizationCommentRequestType As BaseUpdateAuthorizationCommentRequestType) As BaseUpdateAuthorizationCommentResponseType
        Dim oBaseUpdateAuthorizationCommentResponseType As New BaseUpdateAuthorizationCommentResponseType
        Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)

            oBaseUpdateAuthorizationCommentResponseType = UpdateAuthorizationComment(oBaseUpdateAuthorizationCommentRequestType, con)
            Return oBaseUpdateAuthorizationCommentResponseType
        End Using
    End Function
    Public Overloads Function UpdateAuthorizationComment(ByVal oBaseUpdateAuthorizationCommentRequestType As BaseUpdateAuthorizationCommentRequestType, ByVal con As SiriusConnection) As BaseUpdateAuthorizationCommentResponseType
        Try
            Dim oErrors As New SAMErrorCollection
            Dim oResponse As New BaseUpdateAuthorizationCommentResponseType
            oBaseUpdateAuthorizationCommentRequestType.Validate(CObj(oErrors))
            oErrors.CheckForErrors()
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_CashListItem_AuthorizationComment_Update")
                cmd.AddInParameter("@nCashListItem_Id", SqlDbType.Int).Value = oBaseUpdateAuthorizationCommentRequestType.CashListItem_id
                cmd.AddInParameter("@sDescription", SqlDbType.VarChar).Value = oBaseUpdateAuthorizationCommentRequestType.Comment

                con.ExecuteDataSet(cmd, "CashListItem")
            End Using
            Return oResponse
        Catch ex As Exception
            con.RollbackTransaction()
            Throw
        End Try


    End Function

    Public Overloads Function GetAuthorizationComment(ByVal oBaseGetAuthorizationCommentRequestType As BaseGetAuthorizationCommentRequestType) As BaseGetAuthorizationCommentResponseType

        Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
            Dim oBaseGetAuthorizationCommentResponseType As BaseGetAuthorizationCommentResponseType
            oBaseGetAuthorizationCommentResponseType = GetAuthorizationComment(oBaseGetAuthorizationCommentRequestType, con)
            Return oBaseGetAuthorizationCommentResponseType
        End Using
    End Function

    Public Overloads Function GetAuthorizationComment(ByVal oBaseGetAuthorizationCommentRequestType As BaseGetAuthorizationCommentRequestType, ByVal con As SiriusConnection) As BaseGetAuthorizationCommentResponseType
        Try
            Dim oErrors As New SAMErrorCollection
            Dim dsResultSet As DataSet
            Dim oBaseGetAuthorizationCommentResponseType As New BaseGetAuthorizationCommentResponseType
            oBaseGetAuthorizationCommentRequestType.Validate(CObj(oErrors))
            oErrors.CheckForErrors()
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_CashListItem_AuthorizationComment_Get")
                cmd.AddInParameter("@nCashListItem_Id", SqlDbType.Int).Value = oBaseGetAuthorizationCommentRequestType.CashListItem_id
                dsResultSet = con.ExecuteDataSet(cmd, "CashListItem")
            End Using
            If dsResultSet IsNot Nothing AndAlso dsResultSet.Tables.Count > 0 AndAlso dsResultSet.Tables(0).Rows.Count > 0 Then
                With oBaseGetAuthorizationCommentResponseType
                    .Authorization_Comment = Convert.ToString(dsResultSet.Tables(0).Rows(0).Item("authorization_comment"))
                End With
            End If
            Return oBaseGetAuthorizationCommentResponseType
        Catch ex As Exception
            con.RollbackTransaction()
            Throw
        End Try

    End Function

    ''' <summary>
    ''' This method is used to update comment in the cashListItem table for payment authorization
    ''' </summary>
    ''' <param name="oBaseUpdateManualJournalApproversCommentRequestType"></param>
    ''' <returns></returns>
    Public Overloads Function UpdateManualJournalApproversComment(ByVal oBaseUpdateManualJournalApproversCommentRequestType As BaseUpdateManualJournalApproversCommentRequestType) As BaseUpdateManualJournalApproversCommentResponseType
        Dim oBaseUpdateManualJournalApproversCommentResponseType As New BaseUpdateManualJournalApproversCommentResponseType
        Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)

            oBaseUpdateManualJournalApproversCommentResponseType = UpdateManualJournalApproversComment(oBaseUpdateManualJournalApproversCommentRequestType, con)
            Return oBaseUpdateManualJournalApproversCommentResponseType
        End Using
    End Function
    Public Overloads Function UpdateManualJournalApproversComment(ByVal oBaseUpdateManualJournalApproversCommentRequestType As BaseUpdateManualJournalApproversCommentRequestType, ByVal con As SiriusConnection) As BaseUpdateManualJournalApproversCommentResponseType
        Try
            Dim oErrors As New SAMErrorCollection
            Dim oResponse As New BaseUpdateManualJournalApproversCommentResponseType
            oBaseUpdateManualJournalApproversCommentRequestType.Validate(CObj(oErrors))
            oErrors.CheckForErrors()
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_ManualJournal_AuthorizationComment_Update")
                cmd.AddInParameter("@manualJournalId", SqlDbType.Int).Value = oBaseUpdateManualJournalApproversCommentRequestType.ManualJournalId
                cmd.AddInParameter("@sDescription", SqlDbType.VarChar).Value = oBaseUpdateManualJournalApproversCommentRequestType.Comment

                con.ExecuteDataSet(cmd, "ManualJournalItem")
            End Using
            Return oResponse
        Catch ex As Exception
            con.RollbackTransaction()
            Throw
        End Try


    End Function


    ''' <summary>
    ''' Get list of unapproved Manual Journal Transactions Details.
    ''' </summary>
    ''' <param name="oBaseGetListOfManualJournalTransactionMasterRequestType"></param>
    ''' <returns></returns>
    Public Overloads Function GetListofManualJournalTransactionMaster(ByVal oBaseGetListOfManualJournalTransactionMasterRequestType As BaseGetListOfManualJournalTransactionMasterRequestType) As BaseGetListOfManualJournalTransactionMasterResponseType
        Dim oBaseGetListOfManualJournalTransactionMasterResponseType As New BaseGetListOfManualJournalTransactionMasterResponseType
        Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)

            oBaseGetListOfManualJournalTransactionMasterResponseType = GetListOfManualJournalTransactionMaster(oBaseGetListOfManualJournalTransactionMasterRequestType, con)
            Return oBaseGetListOfManualJournalTransactionMasterResponseType
        End Using
    End Function
    Public Overloads Function GetListOfManualJournalTransactionMaster(ByVal oBaseGetListOfManualJournalTransactionMasterRequestType As BaseGetListOfManualJournalTransactionMasterRequestType, ByVal con As SiriusConnection) As BaseGetListOfManualJournalTransactionMasterResponseType
        Try

            Dim oCoreBusiness As New CoreBusiness
            Dim oErrors As New SAMErrorCollection
            Dim dsManualJournalTrans As New DataSet
            Dim dsResultantMasterDataSet As New DataSet
            Dim oBaseGetListOfManualJournalTransactionMasterResponseType As BaseGetListOfManualJournalTransactionMasterResponseType
            oBaseGetListOfManualJournalTransactionMasterRequestType.Validate(CObj(oErrors))
            oErrors.CheckForErrors()
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_Get_UnAuthorised_Manual_Journals_Master")
                cmd.AddInParameter("@manualJournalId", SqlDbType.Int).Value = oBaseGetListOfManualJournalTransactionMasterRequestType.ManualJournalId
                dsManualJournalTrans = con.ExecuteDataSet(cmd, "Row")
            End Using
            If (dsManualJournalTrans.Tables.Count > 0) Then


                If (dsManualJournalTrans.Tables(0).Rows.Count >= 1) Then
                    With dsManualJournalTrans.Tables(0)
                        dsResultantMasterDataSet.Tables.Add()
                        dsResultantMasterDataSet.Tables(0).Columns.Add("CreatedDate", GetType(System.DateTime))
                        dsResultantMasterDataSet.Tables(0).Columns.Add("DocumentType", GetType(System.String))
                        dsResultantMasterDataSet.Tables(0).Columns.Add("Branch", GetType(System.String))
                        dsResultantMasterDataSet.Tables(0).Columns.Add("IsReferred", GetType(System.Int32))
                        dsResultantMasterDataSet.Tables(0).Columns.Add("Comment", GetType(System.String))
                        dsResultantMasterDataSet.Tables(0).Columns.Add("ReversesOn", GetType(System.DateTime))
                        dsResultantMasterDataSet.Tables(0).Columns.Add("RecurringOccurs", GetType(System.Int32))
                        dsResultantMasterDataSet.Tables(0).Columns.Add("PerPeriodOnDay", GetType(System.Int32))
                        dsResultantMasterDataSet.Tables(0).Columns.Add("PerMonthOnDay", GetType(System.Int32))
                        dsResultantMasterDataSet.Tables(0).Columns.Add("PerQuarterOnDay", GetType(System.Int32))
                        dsResultantMasterDataSet.Tables(0).Columns.Add("AuthorisationComment", GetType(System.String))
                        For icounter = 0 To .Rows.Count - 1
                            dsResultantMasterDataSet.Tables(0).Rows.Add()

                            dsResultantMasterDataSet.Tables(0).Rows(icounter)("CreatedDate") = .Rows(icounter).Item("CreatedDate")
                            dsResultantMasterDataSet.Tables(0).Rows(icounter)("DocumentType") = .Rows(icounter).Item("DocumentType")
                            dsResultantMasterDataSet.Tables(0).Rows(icounter)("Branch") = .Rows(icounter).Item("Branch")
                            dsResultantMasterDataSet.Tables(0).Rows(icounter)("IsReferred") = .Rows(icounter).Item("IsReferred")
                            dsResultantMasterDataSet.Tables(0).Rows(icounter)("Comment") = .Rows(icounter).Item("Comment")
                            dsResultantMasterDataSet.Tables(0).Rows(icounter)("ReversesOn") = .Rows(icounter).Item("ReverseDate")
                            dsResultantMasterDataSet.Tables(0).Rows(icounter)("RecurringOccurs") = .Rows(icounter).Item("RecurringOccurs")
                            dsResultantMasterDataSet.Tables(0).Rows(icounter)("PerPeriodOnDay") = .Rows(icounter).Item("PerPeriodOnDay")
                            dsResultantMasterDataSet.Tables(0).Rows(icounter)("PerMonthOnDay") = .Rows(icounter).Item("PerMonthOnDay")
                            dsResultantMasterDataSet.Tables(0).Rows(icounter)("PerQuarterOnDay") = .Rows(icounter).Item("PerQuarterOnDay")
                            dsResultantMasterDataSet.Tables(0).Rows(icounter)("AuthorisationComment") = .Rows(icounter).Item("AuthorisationComment")
                        Next
                    End With
                End If
            End If
            oBaseGetListOfManualJournalTransactionMasterResponseType = New BaseGetListOfManualJournalTransactionMasterResponseType
            Dim Docxml As New System.Xml.XmlDocument
            'dsResultantMasterDataSet.DataSetName = "BaseGetListofManualJournalTransactionDetailsRequestType"
            If (dsResultantMasterDataSet.Tables.Count >= 1) Then
                oBaseGetListOfManualJournalTransactionMasterResponseType.ResultMasterDataSet = dsResultantMasterDataSet

            End If

            Return oBaseGetListOfManualJournalTransactionMasterResponseType
        Catch ex As Exception
            con.RollbackTransaction()
            Throw
        End Try


    End Function

    '' <summary>
    ''' Get list of unapproved Manual Journal Transactions Details.
    ''' </summary>
    ''' <param name="oBaseGetListOfManualJournalTransactionDetailsRequestType"></param>
    ''' <returns></returns>
    Public Overloads Function GetListofManualJournalTransactionDetails(ByVal oBaseGetListOfManualJournalTransactionDetailsRequestType As BaseGetListOfManualJournalTransactionDetailsRequestType) As BaseGetListOfManualJournalTransactionDetailsResponseType
        Dim oBaseGetListOfManualJournalTransactionDetailsResponseType As New BaseGetListOfManualJournalTransactionDetailsResponseType
        Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)

            oBaseGetListOfManualJournalTransactionDetailsResponseType = GetListofManualJournalTransactionDetails(oBaseGetListOfManualJournalTransactionDetailsRequestType, con)
            Return oBaseGetListOfManualJournalTransactionDetailsResponseType
        End Using
    End Function

    Public Overloads Function GetListofManualJournalTransactionDetails(ByVal oBaseGetListOfManualJournalTransactionDetailsRequestType As BaseGetListOfManualJournalTransactionDetailsRequestType, ByVal con As SiriusConnection) As BaseGetListOfManualJournalTransactionDetailsResponseType
        Try
            Dim oCoreBusiness As New CoreBusiness
            Dim oErrors As New SAMErrorCollection
            Dim dsManualJournalTrans As New DataSet
            Dim dsResultantDetailDataSet As New DataSet

            Dim oBaseGetListofManualJournalTransactionDetailsResponseType As BaseGetListOfManualJournalTransactionDetailsResponseType
            oBaseGetListOfManualJournalTransactionDetailsRequestType.Validate(CObj(oErrors))
            oErrors.CheckForErrors()
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_Get_UnAuthorised_Manual_Journals_Details")
                cmd.AddInParameter("@manualJournalId", SqlDbType.Int).Value = oBaseGetListOfManualJournalTransactionDetailsRequestType.ManualJournalId
                dsManualJournalTrans = con.ExecuteDataSet(cmd, "Row")
            End Using


            If (dsManualJournalTrans.Tables.Count > 0) Then

                If (dsManualJournalTrans.Tables(0).Rows.Count >= 1) Then

                    With dsManualJournalTrans.Tables(0)
                        dsResultantDetailDataSet.Tables.Add()
                        dsResultantDetailDataSet.Tables(0).Columns.Add("AccountCode", GetType(System.String))
                        dsResultantDetailDataSet.Tables(0).Columns.Add("ManualJournalDetailId", GetType(System.Int32))
                        dsResultantDetailDataSet.Tables(0).Columns.Add("Amount", GetType(System.Decimal))
                        dsResultantDetailDataSet.Tables(0).Columns.Add("CurrencyCode", GetType(System.String))
                        dsResultantDetailDataSet.Tables(0).Columns.Add("CurrencyTypeDescription", GetType(System.String))
                        dsResultantDetailDataSet.Tables(0).Columns.Add("CurrencyRate", GetType(System.Decimal))
                        dsResultantDetailDataSet.Tables(0).Columns.Add("BaseAmount", GetType(System.Decimal))
                        dsResultantDetailDataSet.Tables(0).Columns.Add("AlternateRef", GetType(System.String))
                        dsResultantDetailDataSet.Tables(0).Columns.Add("Comment", GetType(System.String))
                        dsResultantDetailDataSet.Tables(0).Columns.Add("UnderwritingYearDescription", GetType(System.String))
                        dsResultantDetailDataSet.Tables(0).Columns.Add("CostCentreDescription", GetType(System.String))
                        dsResultantDetailDataSet.Tables(0).Columns.Add("InsuranceRef", GetType(System.String))
                        dsResultantDetailDataSet.Tables(0).Columns.Add("PurchaseOrderNumber", GetType(System.String))
                        dsResultantDetailDataSet.Tables(0).Columns.Add("PurchaseInvoiceNumber", GetType(System.String))
                        dsResultantDetailDataSet.Tables(0).Columns.Add("TransDetailId", GetType(System.Int32))
                        For icount = 0 To .Rows.Count - 1
                            dsResultantDetailDataSet.Tables(0).Rows.Add()
                            dsResultantDetailDataSet.Tables(0).Rows(icount)("AccountCode") = .Rows(icount).Item("AccountCode")
                            dsResultantDetailDataSet.Tables(0).Rows(icount)("ManualJournalDetailId") = .Rows(icount).Item("ManualJournalDetailId")
                            dsResultantDetailDataSet.Tables(0).Rows(icount)("Amount") = .Rows(icount).Item("Amount")
                            dsResultantDetailDataSet.Tables(0).Rows(icount)("CurrencyCode") = .Rows(icount).Item("CurrencyCode")
                            dsResultantDetailDataSet.Tables(0).Rows(icount)("CurrencyRate") = .Rows(icount).Item("CurrencyRate")
                            dsResultantDetailDataSet.Tables(0).Rows(icount)("CurrencyTypeDescription") = .Rows(icount).Item("CurrencyTypeDescription")
                            dsResultantDetailDataSet.Tables(0).Rows(icount)("BaseAmount") = .Rows(icount).Item("BaseAmount")
                            dsResultantDetailDataSet.Tables(0).Rows(icount)("AlternateRef") = .Rows(icount).Item("AlternateRef")
                            dsResultantDetailDataSet.Tables(0).Rows(icount)("Comment") = .Rows(icount).Item("Comment")
                            dsResultantDetailDataSet.Tables(0).Rows(icount)("UnderwritingYearDescription") = .Rows(icount).Item("UnderwritingYearDescription")
                            dsResultantDetailDataSet.Tables(0).Rows(icount)("CostCentreDescription") = .Rows(icount).Item("CostCentreDescription")
                            dsResultantDetailDataSet.Tables(0).Rows(icount)("InsuranceRef") = .Rows(icount).Item("InsuranceRef")
                            dsResultantDetailDataSet.Tables(0).Rows(icount)("PurchaseOrderNumber") = .Rows(icount).Item("PurchaseOrderNumber")
                            dsResultantDetailDataSet.Tables(0).Rows(icount)("PurchaseInvoiceNumber") = .Rows(icount).Item("PurchaseInvoiceNumber")
                            dsResultantDetailDataSet.Tables(0).Rows(icount)("TransDetailId") = .Rows(icount).Item("TransDetailId")

                        Next
                    End With
                End If
            End If

            oBaseGetListofManualJournalTransactionDetailsResponseType = New BaseGetListOfManualJournalTransactionDetailsResponseType
            Dim Docxml As New System.Xml.XmlDocument
            dsResultantDetailDataSet.DataSetName = "BaseGetListofManualJournalTransactionDetails"
            If (dsResultantDetailDataSet.Tables.Count >= 1) Then

                oBaseGetListofManualJournalTransactionDetailsResponseType.ResultDetailDataSet = dsResultantDetailDataSet
            End If

            Return oBaseGetListofManualJournalTransactionDetailsResponseType
        Catch ex As Exception
            con.RollbackTransaction()
            Throw
        End Try

    End Function

    
    Public Overloads Function ValidateAuthorizationSteps(ByVal oBaseValidateAuthorizationStepsRequestType As BaseValidateAuthorizationStepsRequestType) As BaseValidateAuthorizationStepsResponseType

        Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
            Dim oBaseValidateAuthorizationStepsResponseType As BaseValidateAuthorizationStepsResponseType
            oBaseValidateAuthorizationStepsResponseType = ValidateAuthorizationSteps(oBaseValidateAuthorizationStepsRequestType, con)
            Return oBaseValidateAuthorizationStepsResponseType
        End Using
    End Function

    Public Overloads Function ValidateAuthorizationSteps(ByVal oBaseValidateAuthorizationStepsRequestType As BaseValidateAuthorizationStepsRequestType, ByVal con As SiriusConnection) As BaseValidateAuthorizationStepsResponseType
        Try
            Dim oErrors As New SAMErrorCollection
            Dim dsResultSet As DataSet
            Dim oResponse As New BaseValidateAuthorizationStepsResponseType
            oBaseValidateAuthorizationStepsRequestType.Validate(CObj(oErrors))
            oErrors.CheckForErrors()
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_process_authorization_step")
                cmd.AddInParameter("@manualjournal_id", SqlDbType.Int).Value = oBaseValidateAuthorizationStepsRequestType.ManualJournalId
                cmd.AddInParameter("@lApproved", SqlDbType.TinyInt).Value = oBaseValidateAuthorizationStepsRequestType.IsApproved
                cmd.AddInParameter("@source_id", SqlDbType.Int).Value = _SiriusUser.SourceID
                cmd.AddInParameter("@user_id", SqlDbType.Int).Value = _SiriusUser.UserID
                dsResultSet = con.ExecuteDataSet(cmd, "AuthorizationSteps")
            End Using

            Dim docValidateAuthorizationSteps As New System.Xml.XmlDocument
            dsResultSet.DataSetName = "BaseValidateAuthorizationStepsResponseType"

            If oBaseValidateAuthorizationStepsRequestType.WCFSecurityToken = "" Then
                docValidateAuthorizationSteps.LoadXml(dsResultSet.GetXml)
                oResponse.ResultDataset = docValidateAuthorizationSteps.DocumentElement()
            End If

            oResponse.ResultData = dsResultSet
            Return oResponse
        Catch ex As Exception
            con.RollbackTransaction()
            Throw
        End Try

    End Function

End Class

'*************************************************************************************
'Note:This is the new file 
'************************************************************************************
