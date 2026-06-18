SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_get_risk_info_for_policy'
GO

CREATE PROCEDURE spu_get_risk_info_for_policy 
	@insurance_file_cnt  integer,
	@risk_group_id 	integer = NULL  
AS
BEGIN	
	Declare @gis_policy_link_id integer
	Declare @gis_data_model_id  integer
	Declare @gis_scheme_id      integer
	Declare @risk_id            integer
	Declare @gis_screen_id 		integer
	
	SELECT  @gis_policy_link_id = gis_policy_link_id ,  
		@gis_data_model_id  = gis_data_model_id,
		@gis_scheme_id = gis_scheme_id,
		@risk_id = risk_id            	            	
	FROM    gis_policy_link  
	WHERE   insurance_file_cnt = @insurance_file_cnt  

	IF (@risk_group_id IS NULL) or (@risk_group_id <= 0) 		
		-- Get the risk_group_id from gis_qem_usage, based on gis_scheme
	SELECT    @risk_group_id  = risk_group_id
    	FROM      Gis_Qem_Usage  
    	WHERE     gis_scheme_id = @gis_scheme_id  
	
	-- GET THE gis_screen_id
	SELECT @gis_screen_id = gis_screen_id  
	FROM risk_group  
	WHERE risk_group_id = @risk_group_id 	
	
	-- RETURN THE VALUES
	
SELECT  @gis_policy_link_id gis_policy_link_id ,  
    	@gis_data_model_id  gis_data_model_id ,  
    	@gis_scheme_id 	gis_scheme_id ,  
    	@risk_id risk_id ,  
        @gis_screen_id	gis_screen_id    	
END  	







GO