SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_MID_Import_Finalise'
GO

CREATE PROCEDURE spu_MID_Import_Finalise          
	@Batch_Ref as Varchar(20),
	@source_id int = 0,
	@supplier_id int = 0,
	@site_number int = 0,
	@is_loaded int = 0,
	@is_received int = 0         
AS        
Begin    
        
	DECLARE	@Batch_ID    Int,         
			@Mid_Status_ForUpdate  Int,        
			@Mid_Status_Generated Int,          
			@Mid_Batch_Type_Id  INT          
           
	SELECT @Mid_Batch_Type_Id = batch_type_id from batch_type where code =  'MID1'--'MIDI'          
           
	IF ISNULL(@source_id, 0) <> 0
	BEGIN
		SELECT @Batch_ID = batch_id
		FROM Batch
		WHERE batch_ref = @Batch_Ref
			and batch_type_id = @Mid_Batch_Type_Id
			AND company_id = @source_id
			AND interface_code LIKE'%EXPORT%'	
	END
	ELSE
	BEGIN
		SELECT @Batch_ID = batch_id
		FROM Batch
		WHERE batch_ref = @Batch_Ref
			and batch_type_id = @Mid_Batch_Type_Id
			AND company_id = (SELECT  TOP 1 source_id FROM MID_Rule
								WHERE Site_Number = @site_number AND Supplier_id = @supplier_id
								AND Is_Deleted = 0 AND MID_Type = 'MID1') 
			 AND interface_code LIKE'%EXPORT%'	
	END         
           
	If @is_received = 1
		SELECT @Mid_Status_ForUpdate = Mid_Status_ID From MID_Status Where Code='RECEIVED'
	ELSE
		SELECT @Mid_Status_ForUpdate = Mid_Status_ID From MID_Status Where Code='LOADED'

	SELECT @Mid_Status_Generated = Mid_Status_ID From MID_Status Where Code='GENERATED'
           
 Update Mid_Policy Set Mid_Status_ID=@Mid_Status_ForUpdate          
 Where Batch_ID=@Batch_ID And Mid_Status_ID=@Mid_Status_Generated          
 And (reject_Error_Codes Is Null  Or reject_Error_Codes = '')          
 -- (do not change the status if there was a policy error)          
           
 Update MV Set Mid_Status_ID = @Mid_Status_ForUpdate          
 From Mid_Vehicle as MV inner join Mid_Policy as MP on MV.Mid_Policy_ID = MP.Mid_Policy_ID         
 Where MP.Batch_ID = @Batch_ID           
 And MV.Mid_Status_ID=@Mid_Status_Generated          
 And (MV.reject_Error_Codes Is Null  Or MV.reject_Error_Codes = '')         
 -- (do not change the status if there was a error)          
           
End          
  
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
