EXECUTE DDLDropProcedure 'spu_SAM_IsAnniversaryDateEditable'
GO

CREATE PROCEDURE spu_SAM_IsAnniversaryDateEditable 

		@insurance_file_cnt int
		
AS
BEGIN

SELECT ISNULL(Anniversary_Date_Editable,0) FROM Product p 
INNER JOIN insurance_file ifi ON p.product_id = ifi.product_id WHERE insurance_file_cnt = @insurance_file_cnt 

END
GO