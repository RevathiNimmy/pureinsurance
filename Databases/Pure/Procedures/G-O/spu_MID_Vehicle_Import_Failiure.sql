SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_MID_Vehicle_Import_Failiure'
GO


CREATE  PROCEDURE spu_MID_Vehicle_Import_Failiure          
@Batch_Ref    Varchar(20),          
@InsuranceRef as Varchar(30),      
@Registration_Number Varchar(50),          
@Error_Ref    Varchar(35),          
@Errors     Varchar(255) ,
@source_id int = NULL         
As       
Begin       
    
 Declare          
 @Batch_ID   Int,          
 @Mid_Status_ID  Int,          
 @Mid_Batch_Type_Id int          
 --Update the corresponding vehicle record with the error codes and reference.          
 --Finding the vehicle using insurance ref and vehicle reg. Set the status to ERROR          
 select @Mid_Batch_Type_Id = batch_type_id from batch_type where code =  'MID1'--'MIDI'          
           
 Select @Batch_ID= batch_ID from Batch
	WHERE  RIGHT(RTRIM('000000'+ISNULL(batch_ref,'')),6) =  RIGHT(RTRIM('000000'+ISNULL(@Batch_Ref,'')),6)
		And batch_type_id = @Mid_Batch_Type_Id
		And company_id = @source_id
		AND interface_code LIKE'%EXPORT%'
           
 IF CHARINDEX('E',@Errors) > 0
		SELECT @Mid_Status_ID= Mid_Status_ID FROM MID_Status WHERE Code='ERROR'
	ELSE
		SELECT @Mid_Status_ID= Mid_Status_ID FROM MID_Status WHERE Code='LOADED'
 
 DECLARE @MId_Policy_Id INT

 Update MV     
 Set reject_reference = @Error_Ref,  
  reject_error_codes = @Errors,        
  Mid_Status_ID = @Mid_Status_ID,
  @MId_Policy_Id=MP.mid_policy_id          
 From MID_Policy as MP inner join insurance_file as InF on MP.insurance_file_cnt = InF.insurance_file_cnt    
 inner join MID_Vehicle as MV on MP.MID_Policy_Id = MV.MID_Policy_Id        
 Where MP.Batch_Id = @Batch_ID    
  And InF.insurance_ref = @InsuranceRef    
  And MV.registration = @Registration_Number     

UPDATE MID_Policy
	set Mid_Status_ID = @Mid_Status_ID WHERE Mid_policy_id = @MId_Policy_Id

End    
  
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
