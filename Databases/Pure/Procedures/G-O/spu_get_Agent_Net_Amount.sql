SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

EXECUTE DDLDropProcedure 'spu_get_Agent_Net_Amount'
GO
  

CREATE PROCEDURE spu_get_Agent_Net_Amount
    @TransdetailID int,
	@AgentAccountId int
AS


DECLARE @DocumentId int = 0 

select @DocumentId = document_id from Transdetail where transdetail_id = @TransdetailID  

select td.transdetail_id, TD.outstanding_amount,   
(select sum(outstanding_amount)  from transdetail TD2 where TD2.document_id = TD.document_id and TD2.account_id = @AgentAccountId) TotalOutstandingAmount,
td.insurance_ref, 
d.document_ref
 from TransDetail TD
 inner join document D on d.document_id = td.document_id
Where TD.account_id = @AgentAccountId  
and TD.document_id = @DocumentId 





GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO
