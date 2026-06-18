SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

Execute DDLDropProcedure 'spu_ACT_ReleasedAccountsTransactions_Update'
GO

-- Object:  Stored Procedure spu_ACT_ReleasedAccountsTransactions_Update   
-- Script Date: 3/12/2004  

CREATE PROCEDURE spu_ACT_ReleasedAccountsTransactions_Update
        @SuspendedTransdetailId int,
        @DestinationTransdetailId int,
        @AllocationId int,
        @ReleaseDate datetime,
        @RecallDate datetime 
AS

UPDATE   Released_Accounts_Transactions
SET
 
allocation_id=@AllocationId, 
release_date=@ReleaseDate,
recall_date=@RecallDate 
 
WHERE   suspended_transdetail_id = @SuspendedTransdetailID
AND	destination_transdetail_id = @DestinationTransdetailID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

