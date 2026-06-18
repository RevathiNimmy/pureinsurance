SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ACT_Sel_IsFinanced'
GO

CREATE PROCEDURE spu_ACT_Sel_IsFinanced
    @transdetail_id int 

AS
BEGIN

/*Is the policy financed */

DECLARE @client_transdetailID varchar(25),
        @account_id int,
        @premium_finance_cnt int,
        @premium_finance_version int,
	@insurance_file_cnt int,
	@insurance_ref varchar(30) 

SELECT @Client_TransdetailID = 0

SELECT @Client_TransdetailID = (SELECT min(t2.transdetail_id)
			    FROM transdetail t
			    INNER JOIN document d on t.document_id = d.document_id
			    INNER JOIN transdetail t2 on t2.document_id = d.document_id
			    INNER JOIN account a on t2.account_id = a.account_id
			    LEFT JOIN ledger l on l.ledger_id = a.ledger_id
			    WHERE t.transdetail_id = @transdetail_id
			    AND l.ledger_short_name = 'SA')

SELECT @account_id=ex.account_map_id,@insurance_file_cnt=i.insurance_file_cnt,@insurance_ref=i.insurance_ref
			     FROM TransDetail t
			     INNER JOIN document d ON d.document_id = t.document_id
			     INNER JOIN Insurance_File i ON d.insurance_file_cnt = i.insurance_file_cnt
			     INNER JOIN Party p on p.party_cnt = i.broker_cnt
			     INNER JOIN Account a on a.account_key = p.party_cnt 
			     INNER JOIN Element e on a.short_code = e.element_name
		             INNER JOIN Elementextras ex on e.element_id = ex.element_id
                             WHERE t.transdetail_id = @transdetail_id 

SELECT @premium_finance_cnt=pfprem_finance_cnt,
       @premium_finance_version=pfprem_finance_version 
			  FROM PFTransaction_id
			  WHERE pftransaction_id = @Client_TransdetailID

SELECT
	@premium_finance_cnt AS premium_finance_cnt,
	@premium_finance_version AS premium_finance_version,
 	@account_id AS account_id,
	@insurance_file_cnt AS insurance_file_cnt,
	@insurance_ref AS insurance_ref 


END  
GO



 