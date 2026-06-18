SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO
EXECUTE DDLDropProcedure 'spu_SAM_updateparty_via_messaging'
GO
CREATE PROCEDURE spu_SAM_updateparty_via_messaging    
  @Party_Cnt INT,  
  @trading_name varchar(255),  
  @main_contact varchar(255)  
  
AS  
  
DECLARE @Contact_Cnt INT  
DECLARE @ContactType_Id INT  
DECLARE @DateToday DATETIME

SELECT @DateToday = GETDATE()  
  
 Update party  
 set name = @trading_name,  
     resolved_name  = @trading_name
 where party_cnt = @Party_Cnt  
  
SELECT		@Contact_Cnt = C.contact_cnt
	FROM	Party_Contact_Usage PCU  
INNER JOIN Contact C  
   ON PCU.contact_cnt = c.contact_cnt  
   INNER JOIN Contact_Type CT  
   ON		CT.contact_type_id  = c.contact_type_id  
   WHERE	PCU.party_cnt = @Party_Cnt  
   AND		RTRIM(UPPER(CT.code)) = 'MAIN'  
  
If(ISNULL(@Contact_Cnt,0) <> 0 )
	Update Contact  
	Set description = @main_contact  
	where contact_cnt = @Contact_Cnt  
ELSE IF ISNULL(@Contact_Cnt,0) = 0 AND ISNULL(@main_contact,'')<>''
	BEGIN
	
		SELECT    @ContactType_Id = contact_type_id 
		FROM	contact_type
		WHERE RTRIM(UPPER(code)) = 'MAIN'  

		EXEC spe_Contact_add
					@Contact_Cnt=@Contact_Cnt OUTPUT,
					@contact_type_id = @ContactType_Id ,  
					@source_id = 1,  
					@contact_id = 0 ,  
					@country_id = 1,  
					@description = @main_contact ,  
					@area_code = '' ,  
					@number = '' ,  
					@extension ='' ,  
					@created_by_id = 1,  
					@date_created = @DateToday,  
					@modified_by_id = 1 ,  
					@last_modified = NULL  
			
		INSERT INTO party_Contact_usage (party_cnt, contact_cnt) VALUES (@Party_Cnt, @Contact_Cnt)
	END		
		
	

	
	
	