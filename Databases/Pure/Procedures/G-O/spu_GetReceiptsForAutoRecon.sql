SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON

GO

EXECUTE DDLDropProcedure 'spu_GetReceiptsForAutoRecon'

GO

create procedure spu_GetReceiptsForAutoRecon
@AgentAccountId int,
@Amount Numeric(20,4),
@ToleranceAmount Numeric(20,4),
@ToleranceCurrency int

As

BEGIN

select TOP 1 td.transdetail_id, TD.outstanding_amount, td.document_id, d.document_ref, td.accounting_date,  Abs((ABS(TD.outstanding_amount) - ABS(@Amount))) ToleranceGap from TransDetail as TD   
		Left join document as D on D.document_id = TD.document_id    
		Where D.document_ref like 'SRP%'    
		and account_id = @AgentAccountId    
		and TransDetail_id not in (Select transdetail_id from AllocationDetail)    
		AND ABS(TD.outstanding_amount) >= ABS(@Amount - @ToleranceAmount)    
		AND ABS(TD.outstanding_amount) <= ABS(@Amount + @ToleranceAmount) 
		order by  ToleranceGap asc, TD.accounting_date desc  


END
GO

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON

GO