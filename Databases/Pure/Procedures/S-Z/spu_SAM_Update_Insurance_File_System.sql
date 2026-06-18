SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Update_Insurance_File_System'
GO


CREATE PROCEDURE spu_SAM_Update_Insurance_File_System

@user_id integer,  
@insurance_file_cnt integer,  
@transaction_type_code varchar(10),  
@ViaREN INT=0,  
@last_trans_description varchar(255) ='' 
  
AS  
  
 DECLARE @transaction_type_description varchar(255)  
 DECLARE @transaction_type_id integer  
  
IF ISNULL(@last_trans_description,'')<>''
	BEGIN
		SELECT @transaction_type_description=@last_trans_description
	END
ELSE
	BEGIN
SELECT @transaction_type_description = description,  
  @transaction_type_id = transaction_type_id  
 FROM transaction_type  
 WHERE code = @transaction_type_code  
	END
	
 
 UPDATE Insurance_File_System  
 SET modified_by_id = @user_id,  
  last_modified = GetDate(),  
  last_trans_date = GetDate(),  
  last_trans_type_id = ISNULL(@transaction_type_id,last_trans_type_id),  
  last_trans_description = @transaction_type_description  
 
  
 FROM insurance_file_system ifs  
 WHERE insurance_file_cnt = @insurance_file_cnt  
  
 IF @ViaREN=1  
  BEGIN  
   UPDATE Insurance_File_System  
   SET created_by_id=@user_id,
   date_created= GetDate() 
   WHERE insurance_file_cnt = @insurance_file_cnt  
  END  

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
