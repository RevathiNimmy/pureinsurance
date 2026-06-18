SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

Execute DDLDropProcedure 'spu_ACT_ReleasedAccountsTransactions_Add'
Go

--Object:  Stored Procedure dbo.spu_ACT_ReleasedAccountsTransactions_Add   
-- Script Date: 4/12/2004 ******/

CREATE PROCEDURE spu_ACT_ReleasedAccountsTransactions_Add
        @SuspendedTransdetailId int,
        @DestinationTransdetailId int,
  	    @AllocationId int = NULL,
        @ReleaseDate datetime,
        @InstalmentID INT
AS

DECLARE @PFPremFinanceCnt AS INT  
  
IF @AllocationId = 0  
SELECT @AllocationId = NULL  
IF @InstalmentID>0
Begin
	 INSERT INTO Released_Accounts_Transactions  
	(  
	 suspended_transdetail_id,  
	 destination_transdetail_id,  
	 allocation_id,  
	 release_date,  
	 recall_date,  
	 pfinstalments_id)  
	VALUES  
	(  
	 @SuspendedTransdetailId,  
	 @DestinationTransdetailId,  
	 @AllocationId,  
	 @ReleaseDate,  
	 NULL,  
	 @InstalmentID)
End
Else
Begin
	  INSERT INTO Released_Accounts_Transactions  
	(  
	 suspended_transdetail_id,  
	 destination_transdetail_id,  
	 allocation_id,  
	 release_date,  
	 recall_date)  
	VALUES  
	(  
	 @SuspendedTransdetailId,  
	 @DestinationTransdetailId,  
	 @AllocationId,  
	 @ReleaseDate,  
	 NULL)
End
 
SELECT @PFPremFinanceCnt=ISNULL(pfprem_finance_cnt,0)  
FROM suspended_accounts_transactions  
WHERE suspended_transdetail_id = @SuspendedTransdetailId  
  
IF @PFPremFinanceCnt>0  
 UPDATE susp  
 SET susp.is_deleted = 1  
 FROM suspended_accounts_transactions susp  
 LEFT OUTER JOIN transdetail trans on Trans.transdetail_id = susp.suspended_transdetail_id  
 WHERE susp.suspended_transdetail_id = @SuspendedTransdetailId  
 AND trans.outstanding_amount = 0  
ELSE  
 UPDATE susp  
 SET susp.is_deleted = 1  
 FROM suspended_accounts_transactions susp  
 LEFT OUTER JOIN transdetail trans on Trans.transdetail_id = susp.linked_transdetail_id  
 WHERE susp.suspended_transdetail_id = @SuspendedTransdetailId  
 AND (trans.outstanding_amount = 0  
      OR susp.linked_transdetail_id = 0)

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

