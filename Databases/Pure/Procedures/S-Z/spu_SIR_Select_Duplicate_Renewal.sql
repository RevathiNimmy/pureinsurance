SET QUOTED_IDENTIFIER OFF 
Go

EXECUTE DDLDropProcedure 'spu_SIR_Select_Duplicate_Renewal'
GO

CREATE PROCEDURE spu_SIR_Select_Duplicate_Renewal  
 @nInsurance_file_cnt int  
AS  
 Declare @CoverStartDate Date
 Declare @ExpiryDate Date
 Declare @RenewalDate Date
 Declare @insurance_folder_cnt INT
 
select @CoverStartDate = cover_start_date,@ExpiryDate =expiry_date ,@RenewalDate = renewal_date ,@insurance_folder_cnt = insurance_folder_cnt 
from insurance_file where insurance_file_cnt  =@nInsurance_file_cnt

 
select insurance_file_cnt,cover_start_date,expiry_date
from insurance_file where insurance_file_type_id =2 and (insurance_file_status_id IS NULL OR insurance_file_status_id IN (309,4))
and cover_start_date  =@CoverStartDate and expiry_date =@ExpiryDate 
and insurance_folder_cnt = @insurance_folder_cnt 
and insurance_file_cnt <> @nInsurance_file_cnt
SET QUOTED_IDENTIFIER OFF 
GO