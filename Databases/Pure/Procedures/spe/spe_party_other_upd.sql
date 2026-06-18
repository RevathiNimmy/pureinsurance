SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spe_party_other_upd'
GO

CREATE PROCEDURE spe_party_other_upd  
 @party_cnt int,  
 @license_type_id int,  
 @license_number varchar(20),  
 @date_of_birth datetime,  
 @gender varchar(70),  
 @party_status int,  
 @reference_number varchar(20),  
 @external_id int,  
 @reg_number varchar(20),  
 @date_passed_test datetime,  
 @contact_name varchar(255),  
 @contact_telephone_number varchar(255),  
 @insurer_name varchar(255),  
 @insurer_address1 varchar(60),  
 @insurer_address2 varchar(60),  
 @insurer_address3 varchar(60),  
 @insurer_address4 varchar(60),  
 @insurer_postcode varchar(20),  
 @insurer_telephone_number varchar(255),  
 @insurer_fax_number varchar(255),  
 @insurer_contact_name varchar(255),  
 @insurer_email varchar(255),  
 @insurer_notes varchar(2000),  
 @company_notes varchar(2000), 
 @active_indicator bit = NULL, 
 @after_hours_indicator bit  =NULL, 
 @priority_indicator tinyint = NULL,
 @callfromSAM tinyint=NULL,
 @is_TPA_Settle_directly  tinyint=NULL,
 @name varchar(255)=NULL,
 @currency_id int=0,
 @source_id int=0,
 @sub_branch_id int=0,
 @UserId int = NULL,
 @UniqueId VARCHAR(50) = NULL,
 @ScreenHierarchy VARCHAR(500) = NULL

AS  
BEGIN  
  
UPDATE  
 party_other  
SET  
 license_type_id=@license_type_id,  
 license_number=@license_number,  
 date_of_birth=@date_of_birth,  
 gender=@gender,  
 party_status=@party_status,  
 reference_number=@reference_number,  
 external_id=@external_id,  
 reg_number=@reg_number,  
 date_passed_test=@date_passed_test,  
 contact_name=@contact_name,  
 contact_telephone_number=@contact_telephone_number,  
 insurer_name=@insurer_name,  
 insurer_address1=@insurer_address1,  
 insurer_address2=@insurer_address2,  
 insurer_address3=@insurer_address3,  
 insurer_address4=@insurer_address4,  
 insurer_postcode=@insurer_postcode,  
 insurer_telephone_number=@insurer_telephone_number,  
 insurer_fax_number=@insurer_fax_number,  
 insurer_contact_name=@insurer_contact_name,  
 insurer_email=@insurer_email,  
 insurer_notes=@insurer_notes,  
 company_notes=@company_notes , 
 active_indicator=  @active_indicator,
 after_hours_indicator = @after_hours_indicator,
 priority_indicator=  @priority_indicator,
 is_TPA_Settle_directly= Case @callfromSAM  When 1  Then @is_TPA_Settle_directly  Else is_TPA_Settle_directly END,
 UserId = @UserId,
 UniqueId = @UniqueId,
 ScreenHierarchy = @ScreenHierarchy
 
WHERE
 party_cnt = @party_cnt
 
 IF @callfromSAM=1
 BEGIN
 UPDATE Party SET name=@name,resolved_name=@name,source_id=@source_id,sub_branch_id=@sub_branch_id,currency_id=@currency_id WHERE party_cnt = @party_cnt
 END
  
END  


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
