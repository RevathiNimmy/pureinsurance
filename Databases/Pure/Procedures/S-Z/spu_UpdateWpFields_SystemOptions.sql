
SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_UpdateWpFields_SystemOptions'
GO

CREATE Procedure spu_UpdateWpFields_SystemOptions 
AS
    IF EXISTS (SELECT * FROM Hidden_Options WHERE UW_type = 'U') BEGIN
        PRINT 'Underwriting'
    END ELSE
    BEGIN
        
    	Declare @Option_value varchar(100)
    	
    	SET NOCOUNT ON
    	delete from wp_fields where sql='spu_wp_claim_udf'
    	
    	DECLARE cUdfClaimFields CURSOR FAST_FORWARD FOR
    	    select distinct value from system_options where option_number in (2003, 2004, 2005, 2006, 2007) and value <> 0
    	OPEN cUdfClaimFields
    	FETCH NEXT FROM cUdfClaimFields INTO @Option_value
    	WHILE @@FETCH_STATUS = 0 
    	BEGIN
    		Declare @Column_display_name varchar(100)
    		
    		SELECT  @Column_Display_name = cap.caption FROM gis_user_def_header tn, pmcaption cap
    		WHERE tn.GIS_user_def_header_id=@Option_value AND tn.caption_id = cap.caption_id and cap.language_id = 1
    		
		if @Column_Display_Name is not null
		Begin	
	    		INSERT INTO wp_fields (field_name, sql, column_name, column_type, main_group, sub_group, display_name, is_displayed, loop1, loop2, loop3, product_family, data_model, property_id, sub_group2, sub_group3)
	    		VALUES('ClaimUser' + replace(@Column_Display_Name,' ',''), 'spu_wp_claim_udf', replace(@Column_Display_Name,' ','_'), 0, 'Claim', 'Client Comments', @Column_Display_name, 1, NULL, NULL, NULL, 9, NULL, NULL, NULL, NULL)
    		End
		
    	    FETCH NEXT FROM cUdfClaimFields INTO @Option_value
    	END

    	CLOSE cUdfClaimFields
    	
    DEALLOCATE cUdfClaimFields
END 
GO
