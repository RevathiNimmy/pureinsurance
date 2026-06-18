EXECUTE DDLDropProcedure 'spu_insurance_file_associates_upd'
GO

CREATE PROCEDURE spu_insurance_file_associates_upd     
   @Insurance_file_associates_cnt INT,      
   @Party_cnt INT,      
   @Association_type_id INT,      
   @Date_attached DATE,      
   @Date_removed  DATE,      
   @Is_deleted TINYINT,    
   @Is_DelUnConfirmed TINYINT,      
   @Is_AddUnConfirmed TINYINT,      
   @Association_detail VARCHAR(255),      
   @Insurance_file_cnt INT      
AS      
UPDATE  Insurance_file_associates      
		SET      
		Party_cnt=@Party_cnt,      
		Association_type_id=@Association_type_id,      
		date_attached=@Date_attached,      
		date_removed=@Date_removed,      
		Is_Deleted=@Is_deleted,      
		is_DelUnConfirmed=@Is_DelUnConfirmed,      
		is_AddUnConfirmed=@Is_AddUnConfirmed,      
		Association_detail=@Association_detail,      
		Insurance_file_cnt=@Insurance_file_cnt      
WHERE   Insurance_File_cnt = @Insurance_file_cnt AND Insurance_file_associates_cnt=@Insurance_file_associates_cnt

GO

