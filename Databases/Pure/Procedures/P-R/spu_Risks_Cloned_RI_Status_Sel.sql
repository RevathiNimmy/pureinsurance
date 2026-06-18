EXECUTE DDLDropProcedure 'spu_Risks_Cloned_RI_Status_Sel'
GO

CREATE PROCEDURE spu_Risks_Cloned_RI_Status_Sel
@product_id int=0,  
@source_id int=0  

AS  
  
DECLARE @iInsFileTypeLiveID  INT  
DECLARE @iInsFileTypeQuoteID INT  
DECLARE @iInsFileTypeMTAPID  INT  
DECLARE @iInsFileTypeMTATID  INT  
DECLARE @iInsFileTypeRenewalID INT  
DECLARE @iInsFileTypeMTCID INT  
DECLARE @iInsFileTypeMTRID INT  
DECLARE @iInsFileTypeQMTAPID  INT  
DECLARE @iInsFileTypeQMTATID  INT  
DECLARE @iInsFileTypeQMTRID INT  
  
SELECT @iInsFileTypeLiveID  = insurance_file_type_id FROM insurance_file_type WHERE code = 'POLICY'  
SELECT @iInsFileTypeQuoteID = insurance_file_type_id FROM insurance_file_type WHERE code = 'QUOTE'  
SELECT @iInsFileTypeMTAPID  = insurance_file_type_id FROM insurance_file_type WHERE code = 'MTA PERM'  
SELECT @iInsFileTypeMTATID  = insurance_file_type_id FROM insurance_file_type WHERE code = 'MTA TEMP'  
SELECT @iInsFileTypeRenewalID  = insurance_file_type_id FROM insurance_file_type WHERE code = 'RENEWAL'  
SELECT @iInsFileTypeMTCID  = insurance_file_type_id FROM insurance_file_type WHERE code = 'MTACAN'  
SELECT @iInsFileTypeMTRID  = insurance_file_type_id FROM insurance_file_type WHERE code = 'MTAREINS'  
SELECT @iInsFileTypeQMTAPID  = insurance_file_type_id FROM insurance_file_type WHERE code = 'MTAQUOTE'  
SELECT @iInsFileTypeQMTATID  = insurance_file_type_id FROM insurance_file_type WHERE code = 'MTAQTETEMP'  
SELECT @iInsFileTypeQMTRID  = insurance_file_type_id FROM insurance_file_type WHERE code = 'MTAQREINS'  
  
 SELECT DISTINCT i.insurance_file_cnt  INTO #tmpins FROM  insurance_file i
    INNER JOIN insurance_file_risk_link ifr ON i.insurance_file_cnt=ifr.insurance_file_cnt
	INNER JOIN RI_Arrangement r on r.risk_cnt=ifr.risk_cnt
    LEFT JOIN Insurance_File_Cloned_RI_Usage IFCU on IFCU.insurance_file_cnt=i.insurance_file_cnt  
	LEFT JOIN Insurance_File_Clone_log IFCL on IFCL.insurance_file_cnt=i.insurance_file_cnt  
   WHERE
   (i.product_id=@product_id or isnull(@product_id,0)=0)
    AND  (i.source_id=@source_id or isnull(@source_id,0)=0)
	AND Cloned =1   
    AND (i.insurance_file_type_id = @iInsFileTypeLiveID OR i.insurance_file_type_id = @iInsFileTypeQuoteID OR i.insurance_file_type_id = @iInsFileTypeMTAPID OR i.insurance_file_type_id = @iInsFileTypeMTATID OR  
		i.insurance_file_type_id = @iInsFileTypeRenewalID OR i.insurance_file_type_id = @iInsFileTypeMTCID OR i.insurance_file_type_id = @iInsFileTypeMTRID  
        OR i.insurance_file_type_id = @iInsFileTypeQMTAPID OR i.insurance_file_type_id = @iInsFileTypeQMTATID 
        OR i.insurance_file_type_id = @iInsFileTypeQMTRID)    
    AND IFCU.insurance_file_cnt IS NULL  
    AND (IFCL.insurance_file_cnt IS NULL or i.insurance_file_type_id IN (2,5,8,9))    
    
  
SELECT DISTINCT  -- we only want one policy for each risk returned, we'll look thru each risk later  
    ins.insurance_file_cnt  
,   ins.insurance_ref AS 'Policy Number'  
,   ins.insurance_folder_cnt  
,   ins.insurance_file_type_id  
,   par.shortname AS 'Party Shortname'  
,   par.[name] AS 'Party Name'  
,   ins.cover_start_date  
, ift.code  
FROM  
    insurance_file AS ins  
	INNER JOIN #tmpins on #tmpins.insurance_file_cnt=ins.insurance_file_cnt 
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
INNER Join  
 RI_Model As rim ON ri.ri_model_id = rim.ri_model_id  
inner Join  
 RI_Model As xrim ON ri.xol_ri_model_id = xrim.ri_model_id  
 INNER JOIN Insurance_File_Type ift On ift.insurance_file_type_id = ins.insurance_file_type_id  
 LEFT JOIN Insurance_File_Cloned_RI_Usage IFCU on IFCU.insurance_file_cnt=ins.insurance_file_cnt 
 LEFT JOIN Insurance_File_Clone_log IFCL on IFCL.insurance_file_cnt=ins.insurance_file_cnt 
WHERE 
ri.Cloned = 1 AND (rim.ri_model_type <>4 and xrim.ri_model_type <> 4)  
 AND ri.version_id=1
AND ifrl.status_flag IN ('C','D')

UNION 

SELECT DISTINCT  -- we only want one policy for each risk returned, we'll look thru each risk later  
    ins.insurance_file_cnt  
,   ins.insurance_ref AS 'Policy Number'  
,   ins.insurance_folder_cnt  
,   ins.insurance_file_type_id  
,   par.shortname AS 'Party Shortname'  
,   par.[name] AS 'Party Name'  
,   ins.cover_start_date  
, ift.code  
FROM  
    insurance_file AS ins  
	INNER JOIN #tmpins on #tmpins.insurance_file_cnt=ins.insurance_file_cnt   
INNER JOIN insurance_file_risk_link ifrl ON  
 ins.insurance_file_cnt = ifrl.insurance_file_cnt  
INNER JOIN insurance_file_pt_log ifpt ON  
 (ins.insurance_file_cnt = ifpt.insurance_file_cnt AND ifrl.risk_cnt=ifpt.risk_cnt) 
INNER JOIN  
    Risk AS rsk ON ifrl.risk_cnt = rsk.risk_cnt  
INNER JOIN  
    Risk_Type AS rskt ON rskt.risk_type_id = rsk.risk_type_id  
INNER JOIN  
    Party AS par ON par.party_cnt = ins.insured_cnt  
INNER JOIN  
    RI_Arrangement AS ri ON ri.risk_cnt = rsk.risk_cnt  
INNER Join  
 RI_Model As rim ON ri.ri_model_id = rim.ri_model_id  
inner Join  
 RI_Model As xrim ON ri.xol_ri_model_id = xrim.ri_model_id  
 INNER JOIN Insurance_File_Type ift On ift.insurance_file_type_id = ins.insurance_file_type_id  
 LEFT JOIN Insurance_File_Cloned_RI_Usage IFCU on IFCU.insurance_file_cnt=ins.insurance_file_cnt 
 LEFT JOIN Insurance_File_Clone_log IFCL on IFCL.insurance_file_cnt=ins.insurance_file_cnt 
WHERE 
ri.Cloned = 1 AND (rim.ri_model_type NOT IN (4) and xrim.ri_model_type NOT IN (4))  
 AND ri.version_id=2
order by ins.insurance_folder_cnt,ins.insurance_file_cnt  

DROP TABLE #tmpins