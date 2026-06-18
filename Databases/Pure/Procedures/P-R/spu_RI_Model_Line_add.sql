SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_RI_Model_Line_add'
GO

CREATE PROCEDURE spu_RI_Model_Line_add  
    @ri_model_line_id int output,  
    @ri_model_id int,  
    @priority smallint,  
    @number_of_lines DECIMAL(10,2),  
    @line_limit money,  
    @treaty_id int,  
    @share_percent float,
    @lower_limit money,
    @ceding_rate float,
    @Treaty_Type_id int,  
    @is_obligatory  TinyInt =0,  
    @cede_premium_only tinyint,
	@UserId int,  
    @UniqueId varchar (50),  
    @ScreenHierarchy varchar(500),  
    @premium_calculation_basis int,
	@is_VariableQuotaShare TinyInt = 0
AS  
  
    -- Insert record  
	Insert Into RI_Model_Line (  
	ri_model_id,  
	priority,  
	number_of_lines,  
	line_limit,  
	treaty_id,  
	share_percent,
	lower_limit,
	ceding_rate,
	treaty_type_id,  
	is_obligatory,
	cede_premium_only,  
 	UserId,  
 	UniqueId,  
 	ScreenHierarchy,  
 	premium_calculation_basis_Id,
	Is_VariableQuotaShare)             
	Values (@ri_model_id,  
	@priority,  
	@number_of_lines,  
	@line_limit,  
	@treaty_id,  
	@share_percent,
	@lower_limit,
	@ceding_rate,
	@Treaty_Type_id,  
	@is_obligatory,  
	@cede_premium_only,  
 	@UserId,  
 	@UniqueId,  
 	@ScreenHierarchy,  
 	@premium_calculation_basis,
	 @is_VariableQuotaShare)
	  
    Select @ri_model_line_id = SCOPE_IDENTITY()  
	
Go
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
