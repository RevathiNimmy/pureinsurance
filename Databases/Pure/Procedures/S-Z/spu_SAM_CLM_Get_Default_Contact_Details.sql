SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_CLM_Get_Default_Contact_Details'
GO



CREATE PROCEDURE spu_SAM_CLM_Get_Default_Contact_Details

@insurance_file_cnt int, 
@IsInsurer int

AS

IF @IsInsurer = 1
	BEGIN
		SELECT 
			[cnt].code, 
			[con].area_code,
			[con].number, 
			[con].extension

		FROM contact [con]
		
			LEFT JOIN contact_type [cnt] ON 
				[cnt].contact_type_id = [con].contact_type_id
		
			LEFT JOIN party_contact_usage [pcu] ON 
				[pcu].contact_cnt = [con].contact_cnt
		
			LEFT JOIN party [pty] ON 
				[pty].party_cnt = [pcu].party_cnt
	
			LEFT JOIN insurance_file [ifi] ON [ifi].lead_insurer_cnt = [pty].party_cnt	        
		    	
		WHERE [ifi].insurance_file_cnt = @insurance_file_cnt
	END 
ELSE
	BEGIN
		SELECT 
			[cnt].code, 
			[con].area_code,
			[con].number, 
			[con].extension

		FROM contact [con]
		
			LEFT JOIN contact_type [cnt] ON 
				[cnt].contact_type_id = [con].contact_type_id
		
			LEFT JOIN party_contact_usage [pcu] ON 
				[pcu].contact_cnt = [con].contact_cnt
		
			LEFT JOIN party [pty] ON 
				[pty].party_cnt = [pcu].party_cnt
		        
			LEFT JOIN insurance_file [ifi] ON [ifi].insured_cnt = [pty].party_cnt
	
		WHERE [ifi].insurance_file_cnt = @insurance_file_cnt
	END




GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
