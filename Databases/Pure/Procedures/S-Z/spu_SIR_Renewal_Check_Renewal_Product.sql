SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SIR_Renewal_Check_Renewal_Product'
GO


Create Procedure spu_SIR_Renewal_Check_Renewal_Product
	@ifilecnt int,
	@is_valid tinyint Output
As
Begin
	
	Declare @renewal_product_id int,
			@Product_id int,
			@Old_Risk_grpId int,
			@New_Risk_GrpID int,
			@Old_data_modelId int,
			@New_data_modelId int 
	

	Select @is_valid=1

	Select @renewal_product_id = renewal_product_id,
		   @Product_id = product_id 	 
	From Insurance_file 
	Where insurance_file_cnt = @ifilecnt
	
	If @renewal_product_id is NULL
	Return

	--Check if risk group is different
	Select @Old_Risk_grpId = risk_type_group_id From Product_Risk_Type_Group 
	Where Product_id = @Product_id

	Select @New_Risk_GrpID = risk_type_group_id From Product_Risk_Type_Group 
	Where Product_id = @renewal_product_id

	If @Old_Risk_grpId <> @New_Risk_GrpID
	Begin
		Select @is_valid = 0
		Return
	End
	--Check if data model is different	
	Select @Old_data_modelId = gis_data_model_id
	From Gis_Screen gs 
	Inner Join Risk_type rt On rt.gis_screen_id=gs.gis_screen_id
	Inner Join Risk_type_usage rtu On rtu.risk_type_id = rt.risk_type_id
	Inner Join Product_Risk_Type_Group Prtg On Prtg.risk_type_group_id = rtu.risk_type_group_id
	Where Prtg.Product_id = @Product_id
	And Prtg.risk_type_group_id = @Old_Risk_grpId

	Select @New_data_modelId = gis_data_model_id
	From Gis_Screen gs 
	Inner Join Risk_type rt On rt.gis_screen_id=gs.gis_screen_id
	Inner Join Risk_type_usage rtu On rtu.risk_type_id = rt.risk_type_id
	Inner Join Product_Risk_Type_Group Prtg On Prtg.risk_type_group_id = rtu.risk_type_group_id
	Where Prtg.Product_id = @renewal_product_id
	And Prtg.risk_type_group_id = @New_Risk_GrpID

	If @Old_data_modelId <> @New_data_modelId
	Begin
		Select @is_valid = 0
		Return
	End

End

GO