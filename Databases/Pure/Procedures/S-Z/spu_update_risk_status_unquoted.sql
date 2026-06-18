SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_update_risk_status_unquoted'
GO

-- Created: PW211002

CREATE PROCEDURE spu_update_risk_status_unquoted
    @insurance_file_cnt integer
AS

BEGIN      
      
    DECLARE @risk_status integer,      
            @risk_cnt integer,      
     @status_flag varchar      
      
    SELECT @risk_status = risk_status_id      
      FROM risk_status      
     WHERE code = 'UNQUOTED'      
       
    DECLARE c_risk CURSOR FAST_FORWARD FOR      
        SELECT r.risk_cnt, ifrl.status_flag      
          FROM risk r      
    INNER JOIN insurance_file_risk_link ifrl      
            ON r.risk_cnt = ifrl.risk_cnt      
         WHERE insurance_file_cnt = @insurance_file_cnt
		      
    OPEN c_risk      
    FETCH NEXT FROM c_risk INTO @risk_cnt,@status_flag      
    WHILE @@FETCH_STATUS = 0      
    BEGIN      
 IF @status_flag='U'      
 BEGIN      
  INSERT INTO risk      
   (risk_status_id,      
   risk_folder_cnt,      
   accumulation_id,      
   risk_type_id,      
   description,      
   sequence_number,      
   sum_insured_requested,      
   inception_date,      
   expiry_date,      
   is_not_index_linked,      
   is_accumulated,      
   lapsed_reason_id,      
   lapsed_date,      
   lapsed_description,      
   var_data_ref,      
   total_sum_insured,      
   total_annual_premium,      
   total_this_premium,      
   is_ri_at_risk_level,      
   is_auto_reinsured,      
   gis_screen_id,      
   eml_percentage,      
   risk_number,      
   variation_number,      
   is_risk_selected,      
   coverage,      
   insured_item,      
   extensions,      
   pro_rata_rate,      
   premium_this_year,      
   original_risk_status_id,      
   is_discounted,
   is_mandatory_risk)      
  SELECT @risk_status,      
   risk_folder_cnt,      
   accumulation_id,      
   risk_type_id,      
   description,      
   sequence_number,      
   sum_insured_requested,      
   inception_date,      
   expiry_date,      
   is_not_index_linked,      
   is_accumulated,      
   lapsed_reason_id,      
   lapsed_date,      
   lapsed_description,      
   var_data_ref,      
   total_sum_insured,      
   .0000,      
   .0000,      
   is_ri_at_risk_level,      
   is_auto_reinsured,      
   gis_screen_id,      
   eml_percentage,      
   risk_number,      
   variation_number,      
   is_risk_selected,      
   coverage,      
   insured_item,      
   extensions,      
   NULL,      
   premium_this_year,      
   original_risk_status_id,      
   NULL,
   is_mandatory_risk
  FROM risk WHERE risk_cnt=@risk_cnt      
    
  DECLARE @new_risk_cnt INT    
  SELECT @new_risk_cnt=@@IDENTITY    
    
  UPDATE insurance_file_risk_link      
  SET risk_cnt=@new_risk_cnt,      
      status_flag='C',      
      original_risk_cnt=@risk_cnt      
  WHERE risk_cnt=@risk_cnt and insurance_file_cnt=@insurance_file_cnt       
    
  DECLARE @gis_data_model_code VARCHAR(50),    
   @spstr VARCHAR(50),    
   @insurance_folder_cnt INT,
   @Gis_Policy_Link_id INT,
   @NewGis_Policy_Link_id INT,
   @PolicyBinderTable varchar(50),
   @OldPolicyBinderId VARCHAR(50),
   @NewPolicyBinderId INT,
   @PolicyBinderIdName varchar(50) ,
   @WordingTable varchar(50),
   @originalPolicyBinderId int,
   @SQL NVARCHAR(2500),
   @gis_prop_id INT,
   @gis_obj_id INT,
   @doc_template_id INT,
   @seq_id INT,
   @child INT,
   @copy_of_original INT

    
  SELECT @insurance_folder_cnt=insurance_folder_cnt FROM insurance_file WHERE insurance_file_cnt=@insurance_file_cnt    
  SELECT @gis_data_model_code=RTRIM(code) FROM gis_data_model WHERE gis_data_model_id=(SELECT gis_data_model_id FROM GIS_Screen WHERE gis_screen_id=(select gis_screen_id FROM risk WHERE risk_cnt=@risk_cnt))    
  SELECT @spstr='spg_' + @gis_data_model_code + '_copy_dataset'    
  Select @Gis_Policy_Link_id=Gis_Policy_Link_id from Gis_Policy_link Where Risk_id=@risk_cnt
  
 
  
  /* OOS MTA FIX - Dont pass in old_risk_id on anything other than RISK types */
  
   DECLARE @gis_model_type INT
  SELECT @gis_model_type = gis_data_model_type_id FROM gis_data_model WHERE gis_data_model_id=(SELECT gis_data_model_id FROM GIS_Screen WHERE gis_screen_id=(select gis_screen_id FROM risk WHERE risk_cnt=@risk_cnt))    
  
  --Type 1 = RISK
	IF @gis_model_type='1'      
		BEGIN 
			EXEC @spstr @Gis_Policy_Link_id,NULL,@risk_cnt,@insurance_folder_cnt,@new_risk_cnt,NULL,NULL,NULL,NULL
		END
	ELSE
		BEGIN
			EXEC @spstr @Gis_Policy_Link_id,NULL,NULL,@insurance_folder_cnt,@new_risk_cnt,NULL,NULL,NULL,NULL 
		END
	
	/* OOS MTA FIX - Dont pass in old_risk_id on anything other than RISK types */

	
  --EXEC @spstr NULL,@insurance_folder_cnt,@risk_cnt,@insurance_folder_cnt,@new_risk_cnt,NULL,NULL,NULL,NULL
    
  EXEC spu_copy_risk_extras @risk_cnt, @new_risk_cnt 
  /* Copy Standard wording PN 79695 (by Azeej) */
  SELECT @NewGis_Policy_Link_id=Gis_Policy_Link_id from Gis_Policy_link Where Risk_id=@new_risk_cnt
  
  SELECT @PolicyBinderTable = @gis_data_model_code+ '_Policy_binder'
  
  DECLARE @parmDef NVARCHAR(1000) 
  DECLARE @field_value INT 
  SELECT @PolicyBinderTable = @gis_data_model_code+ '_Policy_binder'
  SELECT @OldPolicyBinderId = RTRIM(@PolicyBinderTable) + '_id'

  SET @parmDef = N'@fvalue int out'  
  SELECT @SQL = N'Select TOP 1 @fvalue = ' + @OldPolicyBinderId + ' From ' + @PolicyBinderTable  
  SELECT @SQL = @SQL + ' WHERE gis_policy_link_id = ' + convert(varchar,@Gis_Policy_Link_id)
		--select @SQL
  Exec SP_ExecuteSQL @SQL, @ParmDef, @fvalue = @originalPolicyBinderId out  
   
  SELECT @SQL = N'Select TOP 1 @fvalue = ' + @OldPolicyBinderId + ' From ' + @PolicyBinderTable  
  SELECT @SQL = @SQL + ' WHERE gis_policy_link_id = ' + convert(varchar,@NewGis_Policy_Link_id)
  Exec SP_ExecuteSQL @SQL, @ParmDef, @fvalue = @NewPolicyBinderId out  
  --PRINT(@NewPolicyBinderId)
  SELECT @WordingTable = @gis_data_model_code+'_standard_wording'
  SELECT @PolicyBinderIdName = @gis_data_model_code+'_Policy_binder_id'
  
  CREATE TABLE #TMPTABLE
  (	sequence_id INT,
	document_template_id INT,
	gis_property_id INT,
	gis_object_id INT,
	child INT,
	copy_of_original INT)
   
   
  SELECT @SQL = 'SELECT SW.sequence_id, 
				 SW.document_template_id,
				 SW.gis_property_id, 
				 SW.gis_object_id,SW.child, 
				 DT.copy_of_original 
				 FROM ' +(@WordingTable) +' SW INNER JOIN Document_Template DT
				 ON DT.document_template_id=SW.document_template_id 
				 WHERE SW.'+convert(varchar,@PolicyBinderIdName) +' = ' +convert(varchar,@originalPolicyBinderId)
  --PRINT(@SQL)
  INSERT INTO #TMPTABLE 
  EXEC(@SQL)
 
  
  DECLARE c_StandardWording cursor fast_forward for 
  SELECT * FROM #TMPTABLE 
  OPEN c_StandardWording  
    FETCH NEXT FROM c_StandardWording INTO @seq_id,@doc_template_id,@gis_prop_id,@gis_obj_id,@child,@copy_of_original
    WHILE @@FETCH_STATUS = 0  
    BEGIN 
    SELECT @child = ISNULL(@child,0)
    Exec spu_Copy_RISK_Standard_Wording 
			@data_model = @gis_data_model_code,
			@new_policy_binder = @NewPolicyBinderId,
			@old_policy_binder = @originalPolicyBinderId,
			@gis_prop_id = @gis_prop_id,
			@gis_obj_id = @gis_obj_id,
			@doc_template_id = @doc_template_id,
			@seq_id = @seq_id,
			@isChild = @child
    FETCH NEXT FROM c_StandardWording INTO @seq_id,@doc_template_id,@gis_prop_id,@gis_obj_id,@child,@copy_of_original
	END
  CLOSE c_StandardWording
  DEALLOCATE c_StandardWording
  DROP TABLE #TMPTABLE 
  /*Copy standard wordings pn 79695 by azeej*/ 
 END      
 ELSE      
 BEGIN      
  UPDATE risk      
  SET risk_status_id = @risk_status      
  WHERE risk_cnt = @risk_cnt      
 END      
      
        FETCH NEXT FROM c_risk INTO @risk_cnt,@status_flag      
    END      
    CLOSE c_risk      
    DEALLOCATE c_risk      
      
END
GO