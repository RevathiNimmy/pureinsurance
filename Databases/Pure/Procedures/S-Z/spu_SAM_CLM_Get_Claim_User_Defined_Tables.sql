SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_CLM_Get_Claim_User_Defined_Tables'
GO


CREATE PROCEDURE spu_SAM_CLM_Get_Claim_User_Defined_Tables  
  
@source_id int   
  
AS  


  
SELECT option_number, gis_user_def_header.code, value
FROM system_options  

	LEFT OUTER JOIN gis_user_def_header ON
		system_options.value = gis_user_def_header.gis_user_def_header_id

WHERE option_number >= 2003  
AND option_number <= 2007  
AND branch_id = @source_id




GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
