SET QUOTED_IDENTIFIER OFF    
Go
SET ANSI_NULLS OFF  
GO

EXECUTE DDLDropProcedure 'spu_Suspended_Accounts_Transactions_Upd'
GO

CREATE PROCEDURE spu_Suspended_Accounts_Transactions_Upd
    @insurance_file_cnt INT,
    @linked_transdetail_id INT
AS
UPDATE Suspended_Accounts_Transactions   
SET linked_transdetail_id=@linked_transdetail_id  
WHERE insurance_file_cnt=@insurance_file_cnt   
 
UPDATE Document
SET insurance_file_cnt=@insurance_file_cnt
WHERE document_id in (SELECT D.document_id FROM Document D  
INNER JOIN TransDetail TD ON TD.document_id = D.document_id
WHERE TD.transdetail_id=@linked_transdetail_id)