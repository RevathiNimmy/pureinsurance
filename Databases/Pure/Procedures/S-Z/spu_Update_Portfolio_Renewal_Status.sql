SET QUOTED_IDENTIFIER OFF 
GO

SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_Update_Portfolio_Renewal_Status'
GO


CREATE PROCEDURE spu_Update_Portfolio_Renewal_Status
    @Insurance_file_cnt int  ,
    @new_Insurance_file_cnt int  
AS  
  
	if exists(select * from Renewal_Status where insurance_file_cnt=@Insurance_file_cnt)
	update Renewal_Status set insurance_file_cnt = @new_Insurance_file_cnt where  insurance_file_cnt = @Insurance_file_cnt
if exists(select * from Renewal_Status where renewal_insurance_file_cnt=@Insurance_file_cnt)
	update Renewal_Status set renewal_insurance_file_cnt = @new_Insurance_file_cnt where renewal_insurance_file_cnt=@Insurance_file_cnt