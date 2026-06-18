SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Risks_Deferred_RI_Status_Sel'
GO

CREATE PROCEDURE spu_Risks_Deferred_RI_Status_Sel  
AS  
  
DECLARE @iRIStatusID          int  
DECLARE @iInsFileTypeLiveID   int  
DECLARE @iInsFileTypeQuoteID  int  
DECLARE @iInsFileTypeMTAPID   int  
DECLARE @iInsFileTypeMTATID  int  
DECLARE @iTreatyId  int  
DECLARE @iInsFileTypeCANCID  int
DECLARE @iInsFileTypeREINSID int 
DECLARE @iInsFileTypeUndRenewalID int 

SELECT @iRIStatusID         = risk_status_id         FROM risk_status         WHERE code = 'RIDEFERRED'  
SELECT @iInsFileTypeLiveID  = insurance_file_type_id FROM insurance_file_type WHERE code = 'POLICY'  
SELECT @iInsFileTypeQuoteID = insurance_file_type_id FROM insurance_file_type WHERE code = 'QUOTE'  
SELECT @iInsFileTypeMTAPID  = insurance_file_type_id FROM insurance_file_type WHERE code = 'MTA PERM'  
SELECT @iInsFileTypeMTATID  = insurance_file_type_id FROM insurance_file_type WHERE code = 'MTA TEMP' 
SELECT @iInsFileTypeCANCID  = insurance_file_type_id FROM insurance_file_type WHERE code = 'MTACAN'
SELECT @iInsFileTypeREINSID  = insurance_file_type_id FROM insurance_file_type WHERE code = 'MTAREINS'
SELECT @iInsFileTypeUndRenewalID  = insurance_file_type_id FROM insurance_file_type WHERE code = 'RENEWAL'

--SELECT @iTreatyId = Treaty_Id FROM Treaty WHERE code = 'RETDEFER'  
  
SELECT DISTINCT  -- we only want one policy for each risk returned, we'll look thru each risk later  
    ins.insurance_file_cnt  
,   ins.insurance_ref AS 'Policy Number'  
,   ins.insurance_folder_cnt  
,   ins.insurance_file_type_id  
,   rsk.risk_cnt  
,   rsk.risk_status_id  
,   rsk.[description] AS 'Risk Desc'  
,   rsk.risk_folder_cnt  
,   par.shortname AS 'Party Shortname'  
,   par.[name] AS 'Party Name'  
,   rskt.gis_screen_id  
,   ins.cover_start_date
,ISNULL(ins.insurance_file_status_id,0) AS InsStatus
,   ift.code
--,   CASE ISNULL(ins.insurance_file_status_id,0) WHEN 3 THEN 0
--      ELSE ISNULL(ins.insurance_file_status_id,0)
--	  END  InsStatus

FROM  
    insurance_file AS ins  
INNER JOIN insurance_file_risk_link ifrl ON  
 ins.insurance_file_cnt = ifrl.insurance_file_cnt  
INNER JOIN  
    Risk AS rsk ON ifrl.risk_cnt = rsk.risk_cnt  
INNER JOIN  
    Risk_Type AS rskt ON rskt.risk_type_id = rsk.risk_type_id  
INNER JOIN  
    Party AS par ON par.party_cnt = ins.insured_cnt  
INNER JOIN  
    RI_Arrangement AS ri ON ri.risk_cnt = rsk.risk_cnt  
INNER JOIN  
    RI_Arrangement_Line AS ra ON ri.ri_arrangement_id = ra.ri_arrangement_id  
LEFT JOIN RI_Model as rm ON ri.ri_model_id = rm.ri_model_id  
JOIN Insurance_file_type AS ift ON ins.insurance_file_type_id=ift.insurance_file_type_id
WHERE 

    (ins.insurance_file_type_id = @iInsFileTypeLiveID OR ins.insurance_file_type_id = @iInsFileTypeUndRenewalID OR ins.insurance_file_type_id = @iInsFileTypeMTAPID  
   OR ins.insurance_file_type_id = @iInsFileTypeMTATID OR ins.insurance_file_type_id =@iInsFileTypeREINSID OR ins.insurance_file_type_id =@iInsFileTypeCANCID)  
   AND (ISNULL(ins.insurance_file_status_id, 0) IN(0,1,2,3,4))   
   --AND ins.insurance_file_type_id<>3
   AND rm.ri_model_type = 2  
   AND original_flag=0  
   --And ins.insurance_ref='HEPHOM00104114'
order by ins.insurance_file_cnt 
  

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
