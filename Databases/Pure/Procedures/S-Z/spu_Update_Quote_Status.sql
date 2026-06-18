SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON
GO

EXECUTE DDLDropProcedure 'spu_Update_Quote_Status'
GO

CREATE PROCEDURE spu_Update_Quote_Status  
 @insurance_file_cnt   INT,  
 @quote_status_id INT  
  
AS  
  
UPDATE Insurance_File SET quote_status_id=@quote_status_id  
WHERE insurance_file_cnt = @insurance_file_cnt  

IF @quote_status_id IN (3,4 ,5)
UPDATE Insurance_File SET date_issued = GETDATE() where insurance_file_cnt = @insurance_file_cnt


GO
