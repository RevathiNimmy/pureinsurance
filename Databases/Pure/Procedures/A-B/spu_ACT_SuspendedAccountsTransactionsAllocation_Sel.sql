SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

Execute DDLDropProcedure 'spu_ACT_SuspendedAccountsTransactionsAllocation_Sel'
GO

-- Object:  Stored Procedure spu_ACT_SuspendedAccountsTransactionsAllocation_Sel
-- Script Date: 9/12/2004

CREATE PROCEDURE spu_ACT_SuspendedAccountsTransactionsAllocation_Sel
        @TransdetailId int
AS

DECLARE @MatchedTotal numeric(19,4)

DECLARE @Branch_id int,
	@SysOption16 int,
	@SysOption94 int

select @branch_id = company_id from transdetail where transdetail_id = @TransdetailId
select @SysOption16 = rtrim((select value from system_options
			                  where option_number = 16
					  and branch_id = @branch_id))
select @Sysoption94 = 0

If @sysoption16 =1
	select @sysoption94 = rtrim((select value from system_options
			                  where option_number = 94
					  and branch_id = @branch_id))


SELECT @MatchedTotal =	ISNULL((SELECT SUM(base_match_amount) FROM transmatch
					      WHERE transdetail_id = @TransdetailId),0)
 
SELECT ISNULL((SELECT max(a.allocation_id) FROM allocationdetail a
		       		JOIN transdetail t on t.transdetail_id = a.transdetail_id
				WHERE t.transdetail_id = @transdetailId
				AND (t.amount =@MatchedTotal or @sysoption94 = 1)),0)
  
 
 
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
 

