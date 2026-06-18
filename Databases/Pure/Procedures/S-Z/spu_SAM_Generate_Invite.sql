SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Generate_Invite' 
GO

/*******************************************************************************************************/
/* spu_SAM_Generate_Invite     */                                                                              
/* GEt Details for the SAM Generate invite Method */
/*******************************************************************************************************/

CREATE PROCEDURE spu_SAM_Generate_Invite 
  @Renewal_Insurance_File_Cnt INT
AS

SET NOCOUNT ON

SELECT 
	PT.party_cnt,
	InsFile.insurance_folder_cnt,
	InsFile.insurance_file_cnt,
	RS.renewal_status_cnt,
	RS.insurance_file_cnt Original_insurance_file_cnt,
	Prod.Is_True_Monthly_Policy,
	InsFile.Anniversary_Copy
FROM 
	Renewal_Status RS 
INNER JOIN 
	Insurance_File InsFile 
ON 
	RS.renewal_insurance_file_cnt = InsFile.insurance_file_cnt 
INNER JOIN 
	Insurance_Folder InsFolder
ON
	InsFile.insurance_folder_cnt = InsFolder.insurance_folder_cnt 
INNER JOIN 
	Party PT 
ON
	InsFolder.insurance_holder_cnt = PT.party_cnt 
LEFT JOIN
	Party PTA
ON 
	InsFile.lead_agent_cnt = PTA.party_cnt 
INNER JOIN
	Product Prod
ON 
	InsFile.product_id = Prod.product_id 
WHERE 
	RS.renewal_insurance_file_cnt = @Renewal_Insurance_File_Cnt
	AND
	   (RS.is_invite_printed = 0 
	OR  RS.is_invite_printed IS null
	    ) 
	--Criteria:Status is in AutoReview:Awaiting Renewal Notice Print 
	AND RS.renewal_status_type_id 	  = 2 
	
SET NOCOUNT OFF

GO


