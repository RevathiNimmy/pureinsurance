SET QUOTED_IDENTIFIER ON	SET ANSI_NULLS ON	SET NOCOUNT ON
GO

EXECUTE DDLDropProcedure 'spu_GIS_GetClaimHeaderDetails'
GO

CREATE PROCEDURE  spu_GIS_GetClaimHeaderDetails
   @ID integer,  
   @claim_cnt INT,	
   @claim_year_to_check INT = 0 

AS

 DECLARE @start_date as datetime  

IF @claim_cnt =0 
BEGIN
  
 -- get the start date from the latest real policy (not MTA)  
 set @start_date =  
     (SELECT top 1 ifi2.cover_start_date  
         FROM    insurance_file ifi, insurance_file ifi2  
  WHERE   ifi.insurance_file_cnt = @ID  
  AND ifi2.insurance_file_type_id=2 --policy  
  AND ifi.insurance_folder_cnt = ifi2.insurance_folder_cnt  
  ORDER BY ifi2.policy_version DESC)  


SELECT Claim_number,  
    Loss_FROM_date,  
    0 AS Total_Reserve,  
    c.description,  
       ps.description AS status ,  
       pc.description AS [primary cause] ,  
       sc.description AS [secondary cause]  ,  
       cc.description AS catastrophe ,  
       last_modified_date,  
    NULL AS perils,  
    claim_id,  
    tt.code,
	rsk.description  
FROM claim AS c  
INNER join progress_status AS ps ON ps.progress_status_id=c.claim_status_id  
INNER join primary_cause AS pc ON pc.primary_cause_id=c.primary_cause_id  
LEFT outer join secondary_cause AS sc ON sc.secondary_cause_id=c.secondary_cause_id  
LEFT outer join catastrophe_code AS cc ON cc.catastrophe_code_id=c.catastrophe_code_id  
INNER Join transaction_type AS tt ON c.transaction_type_id=tt.transaction_type_id 
INNER Join Risk as rsk on rsk.risk_cnt=c.risk_type_id
WHERE policy_id in  
  (  
          --get the policies whose cover start dates are withing the search period  
   select ifi2.insurance_file_cnt  
    FROM    insurance_file ifi,  
    insurance_file ifi2  
    inner join progress_status as ps on ps.progress_status_id=c.claim_status_id  
    inner join primary_cause as pc on pc.primary_cause_id=c.primary_cause_id  
    left outer join secondary_cause as sc on sc.secondary_cause_id=c.secondary_cause_id  
    left outer join catastrophe_code as cc on cc.catastrophe_code_id=c.catastrophe_code_id  
    WHERE   ifi.insurance_file_cnt = @ID  
    AND     ifi.insurance_folder_cnt = ifi2.insurance_folder_cnt  
    AND     ifi2.cover_start_date >= dateadd(year,1-@claim_year_to_check,@start_date)  
    AND     ifi2.cover_start_date <= dateadd(day,-1,dateadd(year,2-@claim_year_to_check,@start_date))  
    AND     ifi2.insurance_file_cnt = c.policy_id  
    AND     c.primary_cause_id NOT IN  
    (  
            --this disregards any claims for this product type which are in the Product_Allowed_Causation table  
     SELECT primary_cause_id  
     FROM    Product_Allowed_Causation pac  
     WHERE   ifi2.product_id = pac.product_id  
    )  
  )  
  
END  
ELSE  
  
BEGIN  
SELECT Claim_number,  
       Loss_FROM_date,  
       (SELECT SUM(Initial_reserve + Revised_reserve) FROM Reserve  
  WHERE Claim_peril_id in (Select Claim_peril_id FROM claim_peril  
        WHERE Claim_id=@Claim_cnt)) AS Total_Reserve,  
       c.description,  
       cs.description AS status ,  
       pc.description AS [primary cause] ,  
       sc.description AS [secondary cause]  ,  
       cc.description AS catastrophe ,  
       last_modified_date,  
       NULL AS perils,  
       claim_id,  
       tt.code,
	   rsk.description	  
FROM claim AS c  
INNER join claim_status AS cs ON cs.claim_status_id=c.claim_status_id  
INNER join primary_cause AS pc ON pc.primary_cause_id=c.primary_cause_id  
LEFT outer join secondary_cause AS sc ON sc.secondary_cause_id=c.secondary_cause_id  
LEFT outer join catastrophe_code AS cc ON cc.catastrophe_code_id=c.catastrophe_code_id  
INNER Join transaction_type AS tt ON c.transaction_type_id=tt.transaction_type_id 
INNER Join Risk as rsk on rsk.risk_cnt=c.risk_type_id
WHERE Claim_id = @Claim_cnt  
END  
GO
