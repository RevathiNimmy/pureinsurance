SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON
GO

EXECUTE DDLDropProcedure 'spu_SAM_Check_Quote'
GO

CREATE PROCEDURE spu_SAM_Check_Quote  
	@insurance_file_cnt			INT,
	@bCloneLivePolicy	TINYINT = 0	
	
AS

If @bCloneLivePolicy = 1 
BEGIN

	SELECT ifi.insurance_folder_cnt, 
	1 insurance_file_type_id, 
	ifi.source_id,
	ifi.lead_agent_cnt, p.quote_auto_numbering_id, b.code, p.product_id,p.is_policy_number_at_quote 
	FROM insurance_file ifi
	LEFT JOIN insurance_file_type IFT on ift.insurance_file_type_id = ifi.insurance_file_type_id
	INNER JOIN product p ON ifi.product_id = p.product_id
	INNER JOIN Source b ON ifi.Source_id = b.Source_id  
	WHERE insurance_file_cnt = @insurance_file_cnt 
	AND ift.code in ('POLICY', 'MTA PERM', 'MTA TEMP', 'MTACAN','MTAREINS','VOID', 'VOIDREP','VOIDRENREP')
END 
ELSE
BEGIN

SELECT ifi.insurance_folder_cnt, ifi.insurance_file_type_id, ifi.source_id,
ifi.lead_agent_cnt, p.quote_auto_numbering_id, b.code, p.product_id,p.is_policy_number_at_quote 
FROM insurance_file ifi
INNER JOIN product p ON ifi.product_id = p.product_id
INNER JOIN Source b ON ifi.Source_id = b.Source_id  
WHERE insurance_file_cnt = @insurance_file_cnt 
AND insurance_file_type_id IN (1,3,11)

END 