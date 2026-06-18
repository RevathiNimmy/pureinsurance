SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Lapse_Renewal' 
GO

/*******************************************************************************************************/
/* spu_SAM_Lapse_Renewal     */                                                                              
/* Get Details for the SAM Lapse Renewal Method */
/*******************************************************************************************************/

CREATE PROCEDURE spu_SAM_Lapse_Renewal 
  @Renewal_Insurance_File_Cnt INT
AS

SET NOCOUNT ON

SELECT 
	PT.party_cnt,
	InsFile.insurance_folder_cnt,
	RS.renewal_status_cnt,
	RS.insurance_file_cnt Original_insurance_file_cnt,
	InsFile.insurance_ref
FROM 
	Renewal_Status RS 
INNER JOIN 
	Insurance_File InsFile 
ON 
	RS.renewal_insurance_file_cnt = InsFile.insurance_file_cnt 
INNER JOIN 
	Party PT 
ON
	InsFile.insured_cnt = PT.party_cnt 
LEFT JOIN
	Party PTA
ON 
	InsFile.lead_agent_cnt = PTA.party_cnt 
WHERE 
	RS.renewal_insurance_file_cnt = @renewal_insurance_file_cnt


SET NOCOUNT OFF

GO

