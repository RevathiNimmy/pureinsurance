SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_Claim_DME_Details'
GO

CREATE PROCEDURE spu_CLM_Get_Claim_DME_Details

@claim_id int

AS

	SELECT 
		claim_id, 
		claim_number, 
		policy_number, 
		insurance_folder_cnt, 
		insured_cnt, 
		client_short_name, 
		source_id, 
		claim.description

	FROM claim

		INNER JOIN Insurance_File ON
			Claim.Policy_id = Insurance_File.Insurance_File_Cnt

	WHERE claim_id = @claim_id



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
