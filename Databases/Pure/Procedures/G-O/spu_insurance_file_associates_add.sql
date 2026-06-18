EXECUTE DDLDROPPROCEDURE 'spu_insurance_file_associates_add'
GO

CREATE PROCEDURE spu_insurance_file_associates_add   
   @Insurance_file_associates_cnt INT=0,    
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
BEGIN    
    
	INSERT INTO Insurance_File_Associates 
				(Party_cnt, Association_type_id, date_attached,    
				date_removed, Is_Deleted,Is_DelUnConfirmed, 
				is_AddUnConfirmed, Association_detail, Insurance_file_cnt)    
				VALUES 
				(@party_cnt, @Association_type_id, @Date_attached, @Date_removed,    
				@Is_deleted,@Is_DelUnConfirmed, @is_AddUnConfirmed, @Association_detail, @Insurance_file_cnt)    
    
END 

GO