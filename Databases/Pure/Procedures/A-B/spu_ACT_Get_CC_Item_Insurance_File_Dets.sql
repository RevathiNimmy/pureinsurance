SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Get_CC_Item_Insurance_File_Dets'
GO

CREATE PROCEDURE spu_ACT_Get_CC_Item_Insurance_File_Dets
    @insurance_file_cnt INT
AS

BEGIN

    SELECT bt.code,  
           sl.code,  
           (Select Case 
           When iff.insurance_file_type_id = 2 then 1
           else iff.policy_version end),  
           clm.claim_id,  
           iff.cover_start_date,  
           iff.expiry_date,
	   iff.inception_date_tpi  
      FROM Insurance_File iff  
 LEFT JOIN Business_Type bt ON iff.business_type_id = bt.business_type_id  
 LEFT JOIN Party p ON iff.insured_cnt = p.party_cnt  
 LEFT JOIN Service_Level sl ON p.service_level_id = sl.service_level_id  
 LEFT JOIN Claim clm ON iff.insurance_file_cnt = clm.policy_id  
       AND clm.reported_date >= iff.cover_start_date  
       AND clm.reported_date <= iff.expiry_date  
     WHERE iff.insurance_file_cnt = @insurance_file_cnt 

END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
