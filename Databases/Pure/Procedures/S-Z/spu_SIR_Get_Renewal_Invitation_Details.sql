--**********************************
-- Author : Pankaj Kaushik
--   
-- History: 18/06/2008    
--
-- Task : WR9 Batch Renewals
--***********************************

EXECUTE DDLDropProcedure 'spu_SIR_Get_Renewal_Invitation_Details'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_SIR_Get_Renewal_Invitation_Details  
     @insurance_file_cnt int
   
AS  
	SELECT 
		PT.party_cnt, InsFile.insurance_folder_cnt, 
		InsFile.insurance_file_cnt, RS.renewal_status_cnt, 
		RS.insurance_file_cnt, Prod.Is_True_Monthly_Policy, 
		InsFile.Anniversary_Copy, InsFile.insurance_ref
	FROM Renewal_Status RS
	INNER JOIN Insurance_File InsFile 
	ON RS.renewal_insurance_file_cnt = InsFile.insurance_file_cnt
	INNER JOIN Insurance_Folder InsFolder 
	ON InsFile.insurance_folder_cnt = InsFolder.insurance_folder_cnt
	INNER JOIN Party PT 
	ON InsFolder.insurance_holder_cnt = PT.party_cnt
--	LEFT JOIN Party PTA 
--	ON InsFile.lead_agent_cnt = PTA.party_cnt
	INNER JOIN Product Prod 
	ON InsFile.product_id = Prod.product_id
	WHERE 
		(RS.is_invite_printed = 0 OR  RS.is_invite_printed is null)
		AND RS.renewal_status_type_id = 2
		AND InsFile.insurance_file_cnt = @insurance_file_cnt

GO

