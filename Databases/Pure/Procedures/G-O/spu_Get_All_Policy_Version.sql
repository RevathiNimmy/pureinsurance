SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Get_All_Policy_Version'
GO

CREATE PROCEDURE spu_Get_All_Policy_Version  
	@InsuranceFolderCnt INT,  
	@InsuranceFileCnt  INT,  
	@filterBackdatedVersions INT = 0,
    @filterCancellationQuote INT= 0,  
    @ViaClientManager INT =0  
AS  
  
/******************************************************************************************************  
* Name : sp_Get_All_Policy_Version  
*  
* Desc : get all versions of policy  
*  
* Hist : 26/02/2001 Created - Tinny  
*  
* Ver  : 1.00.0000  
*  
* Note : either insurance_folder_cnt or insurance_file_cnt is present for this to work  
*  
* JMK 12/05/2001    Add Cover start and stop dates  
*                   Filter out Cancelled MTAs  
* SJ  13/09/2002    Do not show records under renewal  
* KR  27/01/2003    Quote Management (117) changes  
* SET 04/02/2003    Amendments cos table udl_mtar has been renamed MTA_Reason  
*                   & added to pmlookup maintenance.  
* ECK 10/07/2003     Included change added to 1.8.5 Branch for PN 1413  
* RAM20040212       Use RiskCode Description, rather than Product Description. PN Issue 9990  
* DC  06/05/2004    PN11785 cater for WhatIf Renewal insurance_type when displaying status  
* Alx 08/10/2004    Added last_trans_description in select  
* Alx 14/10/2004    Added "insurance_file_cnt" in the order clause, for when date is identical  
* RSC 09/09/2008    Added WITH(NOLOCK) to all select statements to avoid object deadlocking  
* RJP 25/09/2008 Restructured to increase performance with multiple policy versions  
* BSJ 24/03/2009 Readded performance fixes  
 *******************************************************************************************************/  
  
DECLARE @underwriting_flag VARCHAR(20)  
  
DECLARE @InsuranceRef VARCHAR(30)  
DECLARE @Event_cnt INT  
DECLARE @policyoptions BIT,  
	 @LevyTax MONEY  
  
SELECT @underwriting_flag = value  
FROM hidden_options WITH(NOLOCK)  
WHERE branch_id = 1 AND option_number = 1  
  
if @InsuranceFolderCnt = 0  
BEGIN  
 SELECT @InsuranceFolderCnt = insurance_folder_cnt  FROM Insurance_File WITH(NOLOCK)  
 WHERE insurance_file_cnt = @InsuranceFileCnt  
END  

if @InsuranceFileCnt = 0  
BEGIN  
 SELECT @InsuranceFileCnt = insurance_file_cnt  FROM Insurance_File WITH(NOLOCK)  
 WHERE insurance_folder_cnt = @InsuranceFolderCnt  
END 
  
SELECT TOP 1 @InsuranceRef =  insurance_ref FROM Insurance_File WITH(NOLOCK)  
WHERE insurance_file_cnt = @InsuranceFileCnt 
 
CREATE TABLE #PolicyPremiums  
(  
 insurance_file_cnt int,  
 premium money  
)  
 
 CREATE TABLE #Event_log  
(  
 insurance_file_cnt INT,  
 Event_cnt INT  
)

  
SELECT @policyoptions = value FROM system_options WITH(NOLOCK) WHERE branch_id = 1 AND option_number = 5021    
--IF @policyoptions = 1    

DECLARE @iInsuranceFileCnt int    
DECLARE  ins_files_cursor CURSOR FAST_FORWARD FOR    
SELECT insurance_file_cnt FROM insurance_file WITH(NOLOCK)    
where insurance_folder_cnt=@InsuranceFolderCnt    
OPEN ins_files_cursor    
FETCH NEXT FROM ins_files_cursor INTO @iInsuranceFileCnt    
WHILE @@FETCH_STATUS = 0    
BEGIN    
    IF @policyoptions = 1
	BEGIN
  DECLARE    
@tax_value money,    
@tax_value1 money,    
@tax_value2 money,    
@fee_value money,    
@this_premium money    
SELECT @Event_cnt = 0    
  -- Get tax    
  SELECT  @tax_value1 = SUM(value)    
  FROM    Tax_Calculation WITH(NOLOCK)    
  WHERE   insurance_file_cnt = @iInsuranceFileCnt    
  AND  risk_cnt IS NULL    
  AND  transtype in ('TTR','TTF','TTIF')    
  SELECT  @tax_value2 = SUM(value)    
  FROM    Tax_Calculation rt WITH(NOLOCK)    
  JOIN    insurance_file_risk_link ifrl WITH(NOLOCK)     ON ifrl.risk_cnt = rt.risk_cnt    
  JOIN    risk r  WITH(NOLOCK)                           ON r.risk_cnt = rt.risk_cnt    
  WHERE   ifrl.insurance_file_cnt = @iInsuranceFileCnt    
  AND     ifrl.status_flag <> 'U'    
  AND     r.is_risk_selected = 1    
  AND  rt.risk_cnt IS NOT NULL    
  AND  transtype in ('TTR','TTF','TTIF')    
  SELECT  @tax_value = ISNULL(@tax_value1, 0) + ISNULL(@tax_value2, 0)    
  -- Get fee    
  SELECT  @fee_value = SUM(currency_amount)    
  FROM    policy_fee_u WITH(NOLOCK)    
  WHERE   insurance_file_cnt = @iInsuranceFileCnt    
  -- Get premium    
  SELECT  @this_premium = this_premium    
  FROM    insurance_file WITH(NOLOCK)    
  WHERE   insurance_file_cnt = @iInsuranceFileCnt    
  -- Get Levy Tax    
  SELECT @LevyTax = SUM(this_premium) FROM peril WITH(NOLOCK)    
  INNER JOIN Peril_type pt WITH(NOLOCK) ON peril.Peril_type_id = pt.Peril_type_id    
  INNER JOIN Insurance_file_risk_link ifrl WITH(NOLOCK) ON ifrl.risk_cnt = peril.risk_cnt    
  WHERE ifrl.insurance_file_cnt = @iInsurancefilecnt AND ifrl.status_flag <> 'U' AND  pt.is_levy_tax = 1    
  INSERT INTO #PolicyPremiums(insurance_file_cnt,premium)    
  VALUES(@iInsuranceFileCnt,ISNULL(@this_premium,0) + ISNULL(@tax_value,0) + ISNULL(@fee_value,0) + ISNULL(@LevyTax,0))  
  END
 
  SELECT @Event_cnt = 0
  IF @Event_cnt=0    
SELECT @Event_cnt= MAX(Event_cnt) FROM event_log WHERE  insurance_file_cnt= @iInsuranceFileCnt AND claim_cnt IS NULL AND is_manual_description =1    
  AND NOT EXISTS(SELECT 1 FROM event_log WHERE insurance_file_cnt= @iInsuranceFileCnt AND description LIKE 'lapse%')    
  IF ISNULL(@Event_cnt,0)=0    
SELECT @Event_cnt=  MAX(event_cnt) FROM event_log  WHERE claim_cnt IS NULL AND insurance_file_cnt=@iInsuranceFileCnt    
INSERT INTO #Event_Log(insurance_file_cnt,Event_cnt) VALUES(@iInsuranceFileCnt,@Event_cnt)    
  FETCH NEXT FROM ins_files_cursor INTO @iInsuranceFileCnt    
END    
CLOSE ins_files_cursor    
DEALLOCATE ins_files_cursor
 
CREATE TABLE #TempTransactionDate 
(transaction_date DateTime, InsurancefileKey Int)
  
INSERT INTO #TempTransactionDate (transaction_date, InsurancefileKey )
SELECT   MAX(doc.document_date) ,IFI.insurance_file_cnt 
FROM document doc 
JOIN stats_folder sf ON sf.document_ref = doc.document_ref
JOIN Insurance_File IFI ON SF.insurance_file_cnt = IFI.insurance_file_cnt 
WHERE   ifi.insurance_folder_cnt =@InsuranceFolderCnt AND SF.transaction_type_code NOT IN ('DRIC','DRI')      
GROUP BY IFI.insurance_file_cnt 
 
  
SELECT  ifi.insurance_folder_cnt,  
		ifi.insurance_file_cnt,  
		ifo.insurance_holder_cnt,  
		ifi.policy_type_id,  
		CASE (SELECT COUNT(source_id)  
	 FROM source WITH(NOLOCK)  
	 WHERE source_id = ifi.source_id  
	AND underwriting_branch_ind =1  
	AND ifi.alternate_reference IS NOT NULL)  
	 WHEN 0 THEN  
	 ifi.insurance_ref  
	 ELSE  
	 ifi.alternate_reference  
		END,  
		ift.description,  
		CASE @underwriting_flag WHEN 'U' THEN prd.description ELSE rc.description END,  
		ifi.renewal_date,  
		CASE pty.resolved_name  
			WHEN '' THEN pty.name  
			WHEN NULL THEN pty.name  
			ELSE pty.resolved_name  
		END,  
		pty.shortname,  
		CASE @policyoptions WHEN 1 THEN pp.premium ELSE ifi.this_premium END,  
		CASE  
   WHEN @underwriting_flag = 'U'  
	THEN ifs.description  
   WHEN (ifs.description = '' OR (ifs.description IS NULL)) AND  
	(ifi.insurance_file_type_id <> 2 AND ifi.insurance_file_type_id <> 5 AND ifi.insurance_file_type_id <> 6 AND ifi.insurance_file_type_id <> 12)  
	THEN  'Incomplete'  
   ELSE  ifs.description  
  END AS DESCRIPTION,  
 ifi.insurance_file_type_id,  
 ifi.cover_start_date,  
 ifi.expiry_date,  
 um.description,  
 ifi.tax_amount,  
 prd.grace_period,  
 prd.product_id,  
 ifi.annual_premium,  
 ifi.lapsed_date,  
 s.description,  
 s.is_deleted,  
 s.closed_allow_temp_mta,  
 s.closed_allow_perm_mta,  
 CASE                                                          
      When ifsys.last_trans_description like '%Claim%' then ''          
   ELSE ifsys.last_trans_description    
 END,  
 ifi.base_insurance_file_cnt,  
 ISNULL(ISNULL((SELECT TOP 1 document_date FROM document WITH(NOLOCK) WHERE documenttype_id  
                         IN (4, 5, 15, 16, 17, 18, 52, 53) AND insurance_file_cnt = ifi.insurance_file_cnt order by 1 desc 
                       ),ifsys.last_modified), ifsys.last_trans_date),  
 event_log.description,  
 ifi.policy_version AS version  
 FROM    Insurance_File ifi WITH(NOLOCK)  
 INNER JOIN insurance_file_system ifsys WITH(NOLOCK) ON ifi.insurance_file_cnt = ifsys.insurance_file_cnt  
 INNER JOIN Insurance_Folder ifo WITH(NOLOCK) ON ifi.insurance_folder_cnt = ifo.insurance_folder_cnt  
 INNER JOIN Party pty WITH(NOLOCK) ON ifo.insurance_holder_cnt = pty.party_cnt  
 INNER JOIN Product prd WITH(NOLOCK) ON ifi.product_id = prd.product_id  
 INNER JOIN Insurance_File_Type ift WITH(NOLOCK) ON ifi.insurance_file_type_id = ift.insurance_file_type_id  
 INNER JOIN source s WITH(NOLOCK) ON ifi.source_id = s.source_id  
 LEFT JOIN #PolicyPremiums pp WITH(NOLOCK) ON ifi.insurance_file_cnt = pp.insurance_file_cnt  
 LEFT JOIN Insurance_File_Status ifs WITH(NOLOCK) ON ifi.insurance_file_status_id = ifs.insurance_file_status_id  
 LEFT JOIN MTA_Reason um WITH(NOLOCK) ON ifi.user_defined_data_id = um.MTA_Reason_id  
 LEFT JOIN Risk_Code rc WITH(NOLOCK) ON ifi.Risk_Code_ID = rc.Risk_Code_ID  
 LEFT JOIN #Event_Log WITH(NOLOCK) ON #event_log.insurance_file_cnt = ifi.insurance_file_cnt  
 LEFT JOIN event_log WITH(NOLOCK) ON event_log.event_cnt = #event_log.event_cnt   
  
 WHERE   ifi.insurance_folder_cnt = @InsuranceFolderCnt  
  
  AND (ifi.insurance_ref=@Insuranceref)  
  AND     (ifi.insurance_file_type_id <> (select insurance_file_type_id from insurance_file_type where code = 'RENEWAL') OR  @underwriting_flag = 'U')  
  AND  ifi.policy_ignore is null  
  AND ((@ViaClientManager=1) OR ((ifi.insurance_file_type_id NOT IN (1,4,7,10) AND @filterCancellationquote=1) OR (@filterCancellationquote=0 AND ifi.insurance_file_type_id <> 12)))   
  
 
  AND (  
  (@filterBackdatedVersions = 0)  
  OR  
  (@filterBackdatedVersions = 1 AND ISNULL(ifi.out_of_sequence_replaced, 0) = 0)  
 )  
AND (  
  (@filterBackdatedVersions = 0)  
  OR  
  (@filterBackdatedVersions = 1 AND (ISNULL(ifi.insurance_file_status_id, 0) <> 1 OR ift.code NOT IN ('MTAQUOTE','MTAQTETEMP','MTAQREINS','MTAQCAN')))  
 )  
 ORDER BY ifi.cover_start_date DESC,ifi.insurance_file_cnt DESC  
  
DROP TABLE #PolicyPremiums  
  
DROP TABLE #TempTransactionDate  

SET QUOTED_IDENTIFIER OFF
Go
SET ANSI_NULLS ON
GO
