EXECUTE DDLDropProcedure 'spu_ACT_Get_TransDetailID_AccountID'
GO

CREATE PROCEDURE spu_ACT_Get_TransDetailID_AccountID
    @transaction_export_folder_cnt int
AS

select TD.transdetail_id, 
	TD.account_id,
	TD.amount,
	DOC.document_ref,
	TD.document_sequence
from transdetail TD,
     Document DOC,
     transaction_export_folder TEF
where TD.document_id = DOC.document_id
AND DOC.insurance_file_cnt = TEF.insurance_file_cnt
AND TEF.transaction_export_folder_cnt = @transaction_export_folder_cnt
order by document_sequence
	
Go