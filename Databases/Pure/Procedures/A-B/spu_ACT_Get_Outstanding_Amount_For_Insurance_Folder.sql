SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Get_Outstanding_Amount_For_Insurance_Folder'
GO

CREATE PROCEDURE spu_ACT_Get_Outstanding_Amount_For_Insurance_Folder

@insurance_file_cnt int, 
@status int = 0 OUTPUT 

AS

DECLARE @account_id int
DECLARE @insurance_ref varchar(30)

SELECT 
	@account_id = account.Account_id, 
	@insurance_ref = insurance_file.insurance_ref
FROM insurance_file 
	LEFT JOIN Account ON 
		Account.Account_Key = lead_agent_cnt

WHERE insurance_file_cnt = @insurance_file_cnt

IF ISNULL(@account_id, 0) = 0 

SELECT 
	@account_id = account.Account_id
FROM insurance_file 

	INNER JOIN insurance_folder ON
		insurance_file.insurance_folder_cnt = insurance_folder.insurance_folder_cnt
	INNER JOIN Account ON 
		Account.Account_Key = insurance_folder.insurance_holder_cnt

WHERE insurance_file_cnt = @insurance_file_cnt	

IF ISNULL(@account_id, 0) = 0 
 SET @status = -1
ELSE

 SELECT SUM(outstanding_amount)
 FROM Transdetail
 WHERE insurance_ref = @insurance_ref
 AND account_id = @account_id
 


GO
