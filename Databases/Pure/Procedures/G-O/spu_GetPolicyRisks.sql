SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_GetPolicyRisks'
GO

CREATE PROCEDURE spu_GetPolicyRisks  
    @insurance_file_cnt int,  
    @isviaSAM int=0  
AS  
  
    -- Get all risks for this version of the policy with the start and end date from the policy version  
    -- on which the risk was created  
    --  
    -- example  
    -- policy    risk    status_flag  
    -- 956       932     C  
    -- 961       932     U  
    -- 961       938     C  
    -- 962       932     U  
    --  
    -- get all risks for policy 961 will result in risk 932 with start and end date from policy 956  
    -- and risk 938 with start and end date from policy 961  
 
 If Object_ID('#temp_Rating_Section') Is Null
	Create Table #temp_Rating_Section (
		risk_cnt	int,
		rating_section_id	int,
		rating_section_type_id	int,
		policy_section_type_id	int,
		sequence_number	int,
		description	varchar(30),
		rate_type_id	int,
		annual_rate	numeric(21,6),
		sum_insured	numeric(19,4),
		annual_premium	numeric(19,4),
		this_premium	numeric(19,4),
		original_flag	tinyint,
		currency_id	smallint,
		country_id	int,
		state_id	int,
		this_discount	numeric(19,4),
		applied_discount	numeric(19,4),
		adjusted_discount	numeric(4,4),
		is_amended	tinyint,
		calculated_premium	money,
		override_reason	varchar(255),
		discount_original_this_premium	money,
		auto_calculated	tinyint,
		Earning_Pattern_id	int
)
Else
	Delete #temp_Rating_Section

 IF EXISTS (Select * from Rating_Section where original_flag = 1 and risk_cnt in (select risk_cnt from insurance_file_risk_link where insurance_file_cnt = @insurance_file_cnt))  
  BEGIN  
   Insert Into #temp_Rating_Section select risk_cnt	,
		rating_section_id	,
		rating_section_type_id	,
		policy_section_type_id	,
		sequence_number	,
		description	,
		rate_type_id	,
		annual_rate	,
		sum_insured	,
		annual_premium	,
		this_premium	,
		original_flag	,
		currency_id	,
		country_id	,
		state_id	,
		this_discount	,
		applied_discount	,
		adjusted_discount	,
		is_amended	,
		calculated_premium	,
		override_reason	,
		discount_original_this_premium	,
		auto_calculated	,
		Earning_Pattern_id	 from Rating_Section where original_flag = 1 and sequence_number = 1 and risk_cnt in (select risk_cnt from insurance_file_risk_link where insurance_file_cnt = @insurance_file_cnt)  
  END  
 ELSE  
  BEGIN  
   Insert Into #temp_Rating_Section select risk_cnt	,
		rating_section_id	,
		rating_section_type_id	,
		policy_section_type_id	,
		sequence_number	,
		description	,
		rate_type_id	,
		annual_rate	,
		sum_insured	,
		annual_premium	,
		this_premium	,
		original_flag	,
		currency_id	,
		country_id	,
		state_id	,
		this_discount	,
		applied_discount	,
		adjusted_discount	,
		is_amended	,
		calculated_premium	,
		override_reason	,
		discount_original_this_premium	,
		auto_calculated	,
		Earning_Pattern_id	 from Rating_Section where risk_cnt in (select risk_cnt from insurance_file_risk_link where insurance_file_cnt = @insurance_file_cnt)  
  END 
  
 /*    SQL will complain that temp_Rating_Section already exists in the second insert statement
 IF EXISTS (Select * from Rating_Section where original_flag = 1 and risk_cnt in (select risk_cnt from insurance_file_risk_link where insurance_file_cnt = @insurance_file_cnt))  
  BEGIN  
   Select * INTO temp_Rating_Section  from Rating_Section where original_flag = 1 and sequence_number = 1 and risk_cnt in (select risk_cnt from insurance_file_risk_link where insurance_file_cnt = @insurance_file_cnt)  
  END  
 ELSE  
  BEGIN  
   Select * INTO temp_Rating_Section from Rating_Section where risk_cnt in (select risk_cnt from insurance_file_risk_link where insurance_file_cnt = @insurance_file_cnt)  
  END  
 */

    Declare @void_Insurance_file_status_id int 
    
    Select TOP 1 @void_Insurance_file_status_id = insurance_file_status_id from insurance_file_status where code ='VOID'
 
    SET @void_Insurance_file_status_id = isnull(@void_Insurance_file_status_id,0)

 
    IF @insurance_file_cnt = 0  
  
        SELECT  irl1.insurance_file_cnt,  
                rsk.risk_cnt,  
                rsk.description as riskdescription,  
                rty.description,  
                ifi.cover_start_date,  
                ifi.expiry_date,  
    (CASE WHEN (irl1.Status_flag ='D' AND rst.code='QUOTED' AND @isviaSAM = 1) THEN 'DELETED'  
     ELSE  CASE WHEN (ifpt.status_id=1  or ifct.status_id=1)  THEN 'Pending Reinsurance' ELSE rst.description  END  
    END  ) statusdescription,  
                rsk.total_sum_insured,  
                rsk.total_this_premium total_this_premium,  
                rsk.gis_screen_id,  
                rsk.risk_type_id,  
                ifi.insurance_folder_cnt,  
                irl1.status_flag,  
                rsk.risk_number,  
                rsk.variation_number,  
                rsk.is_risk_selected,  
                rsk.coverage,  
                rsk.insured_item,  
                rsk.extensions,  
                ISNULL(risk_taxes.totaltax,0)+ISNULL(levy_tax.totallevytax,0) + ISNULL(totalstampinsured,0)  risk_tax,  
                fee_taxes.totaltax fee_tax,  
                risk_fees.totalfees fee_premium,  
                (CASE WHEN (irl1.Status_flag ='D' AND rst.code='QUOTED' AND @isviaSAM = 1) THEN 'DELETED'  
     ELSE  CASE WHEN (ifpt.status_id=1 or ifct.status_id=1) THEN 'PENDINGRI' ELSE rst.code  END  
    END  ) risk_status_code,  
    rsk.is_discounted,  
    rsk.risk_folder_cnt,  
    rcnl.Risk_Cover_Note_Link_Id,  
    rcnl.Cover_Note_Ref,  
    rcnl.Cover_Note_From,  
    rcnl.Cover_Note_To,  
    ifi.insured_cnt,  
    ifi.lead_agent_cnt,  
    rty.code as risk_type_code,  
    p.Cover_note_Doc_template_id,  
    dt.Document_type_id,  
    dt.is_editable_after_merging,  
    rsk.inception_date, --This parameter is added as per Tech Spec  
    totalstampinsurer,  
    totalstampinsured,  
    irl1.Original_risk_cnt,p.Cover_Note_Reused_Upto,  
    rsk.Is_Mandatory_Risk,  
    IRL1.is_risk_edited  
        FROM    insurance_file_risk_link irl1  
        JOIN    insurance_file_risk_link irl2                ON irl1.risk_cnt = irl2.risk_cnt  
        JOIN    insurance_file ifi                           ON ifi.insurance_file_cnt = irl2.insurance_file_cnt  
        JOIN    risk rsk                                     ON irl2.risk_cnt = rsk.risk_cnt  
        JOIN    risk_type rty                                ON rsk.risk_type_id = rty.risk_type_id  
  LEFT JOIN  Risk_Cover_Note_Link rcnl                   ON rcnl.Risk_Id = rsk.risk_cnt  
  LEFT JOIN   product  p            ON p.product_id=ifi.product_id  
  LEFT JOIN    Document_template dt       ON dt.Document_template_id=p.Cover_note_Doc_template_id  
        LEFT JOIN                  risk_status rst                              ON rst.risk_status_id = rsk.risk_status_id  
        LEFT JOIN  
               (SELECT  risk_cnt, sum(value) totaltax  
                FROM    tax_calculation  
                WHERE   transtype = 'TTF'  
                AND     insurance_file_cnt = @insurance_file_cnt  
                GROUP BY risk_cnt  
                ) fee_taxes                                  ON irl1.risk_cnt = fee_taxes.risk_cnt  
        LEFT JOIN  
               (SELECT  risk_cnt, sum(value) totaltax  
                FROM    tax_calculation  
                WHERE   transtype = 'TTR'  
                GROUP BY risk_cnt  
                ) risk_taxes        ON irl1.risk_cnt = risk_taxes.risk_cnt  
        LEFT JOIN  
               (SELECT  risk_cnt, sum(currency_amount) totalfees  
                FROM    policy_fee_u  
                WHERE   insurance_file_cnt = @insurance_file_cnt  
                GROUP BY risk_cnt  
                ) risk_fees                                  ON irl1.risk_cnt = risk_fees.risk_cnt  
        LEFT JOIN  
               (SELECT  risk_cnt, sum(this_premium) totallevytax  
                FROM    Peril  
      WHERE is_levy_tax=1 AND is_premium=0 AND is_taxed IS NULL  
                GROUP BY risk_cnt  
                ) levy_tax                                  ON irl1.risk_cnt = levy_tax.risk_cnt  
  --Start - Sankar - (WR29 - Stamp Duty Process) - Paralleling  
   LEFT JOIN  
    (SELECT  risk_cnt, sum(this_premium) totalstampinsurer  
   FROM    Peril p  
        JOIN peril_type pt ON p.peril_type_id = pt.peril_type_id where isnull(pt.is_stamp_duty_insurer,0) = 1  
         GROUP BY p.risk_cnt  
                 ) stampinsurer               ON irl1.risk_cnt = stampinsurer.risk_cnt  
     LEFT JOIN  
     (SELECT  risk_cnt, sum(this_premium) totalstampinsured  
             FROM    Peril p  
         JOIN peril_type pt ON p.peril_type_id = pt.peril_type_id where isnull(pt.is_stamp_duty_insured,0) = 1  
                 GROUP BY p.risk_cnt  
                 ) stampinsured               ON irl1.risk_cnt = stampinsured.risk_cnt  
      LEFT JOIN insurance_file_pt_log ifpt on ifpt.risk_cnt=rsk.risk_cnt  
      LEFT JOIN insurance_file_clone_log ifct on ifct.risk_cnt=rsk.risk_cnt  
  
        WHERE   ISNULL(ifi.insurance_file_status_id, 2) IN (1,2,3,4,5,6,309,@void_Insurance_file_status_id)  
        AND     irl2.status_flag NOT IN ('U','R')  
        ORDER BY rsk.risk_cnt  
  
    ELSE  
  
 CREATE TABLE #InsuranceFileRiskLink  
 (  
  RiskCnt INT ,  
  StatusFlag  VARCHAR(1)  
 )  
 INSERT INTO #InsuranceFileRiskLink  
 SELECT IFL2.risk_cnt,IFL2.status_flag from Insurance_file_risk_link IFL1  
 JOIN Insurance_file_risk_link IFL2 ON IFL1.risk_cnt = IFL2.risk_cnt where IFL1.insurance_file_cnt = @insurance_file_cnt  
 AND IFL2.status_flag <> 'U'  
  
        SELECT DISTINCT  
    irl1.insurance_file_cnt,  
                rsk.risk_cnt,  
                 rsk.description as riskdescription,  
                rty.description,  
                ifi.cover_start_date,  
                ifi.expiry_date,  
                (CASE WHEN (irl1.Status_flag ='D' AND rst.code='QUOTED' AND @isviaSAM = 1) THEN 'DELETED'  
       ELSE  CASE WHEN (ifpt.status_id=1  or ifct.status_id=1)  THEN 'Pending Reinsurance' ELSE rst.description  END  
    END  )statusdescription,  
                rsk.total_sum_insured,  
                rsk.total_this_premium  total_this_premium,  
                rsk.gis_screen_id,  
                rsk.risk_type_id,  
                ifi.insurance_folder_cnt,  
                irl1.status_flag,  
                rsk.risk_number,  
                rsk.variation_number,  
                rsk.is_risk_selected,  
                rsk.coverage,  
                rsk.insured_item,  
                rsk.extensions,  
                ISNULL(risk_taxes.totaltax,0)+ISNULL(levy_tax.totallevytax,0) + ISNULL(totalstampinsured,0)  risk_tax,  
                fee_taxes.totaltax fee_tax,  
                risk_fees.totalfees fee_premium,  
                (CASE WHEN (irl1.Status_flag ='D' AND rst.code='QUOTED' AND @isviaSAM = 1) THEN 'DELETED'  
       ELSE  CASE WHEN (ifpt.status_id=1 or ifct.status_id=1) THEN 'PENDINGRI' ELSE rst.code  END  
    END  )  risk_status_code,  
  rsk.is_discounted,  
  
  rsk.risk_folder_cnt  ,  
  rcnl.Risk_Cover_Note_Link_Id,  
  rcnl.Cover_Note_Ref,  
  rcnl.Cover_Note_From,  
  rcnl.Cover_Note_To,  
  ifi.insured_cnt,  
  ifi.lead_agent_cnt,  
  rty.code as risk_type_code,  
  p.Cover_note_Doc_template_id,  
  dt.Document_type_id,  
  dt.is_editable_after_merging,  
  rsk.inception_date, --This parameter is added as per Tech Spec  
  totalstampinsurer,  
  totalstampinsured,  
  irl1.Original_risk_cnt,p.Cover_Note_Reused_Upto,  
  rsk.Is_Mandatory_Risk,  
  IRL1.is_risk_edited,  
  irl1.status_flag 'risk_link_status_flag',  
  [LastVersion].cover_start_date 'risk_link_change_date',  
  CASE
  WHEN (ISNULL((SELECT TOP 1 rating_section_id FROM Rating_Section WHERE risk_cnt=RS.Risk_Cnt and is_amended=1),-1)) > 0    
  THEN 0
  ELSE 1
  END AS Is_Auto_Rated  
        FROM insurance_file ifi  
        INNER JOIN insurance_file_risk_link irl1  
JOIN    #InsuranceFileRiskLink irl2  ON irl1.risk_cnt = irl2.RiskCnt  
        INNER JOIN risk rsk  
        INNER JOIN  risk_type rty  
         ON rsk.risk_type_id = rty.risk_type_id  
  ON irl1.risk_cnt = rsk.risk_cnt  
                ON ifi.insurance_file_cnt = irl1.insurance_file_cnt  
      LEFT JOIN   Risk_Cover_Note_Link rcnl  
                ON rcnl.Risk_Id = rsk.risk_cnt  
      LEFT JOIN product  p  
                ON p.product_id=ifi.product_id  
      LEFT JOIN Document_template dt  
                ON dt.Document_template_id=p.Cover_note_Doc_template_id  
      LEFT JOIN risk_status rst  
                ON rst.risk_status_id = rsk.risk_status_id  
      LEFT JOIN  
               (SELECT  risk_cnt, sum(value) totaltax  
                FROM    tax_calculation  
                WHERE   transtype = 'TTF'  
                AND     insurance_file_cnt = @insurance_file_cnt  
                GROUP BY risk_cnt  
                ) fee_taxes  
                ON irl1.risk_cnt = fee_taxes.risk_cnt  
     LEFT JOIN  (SELECT  insurance_file_cnt,risk_cnt, sum(value) totaltax  
                  FROM    tax_calculation  
                  WHERE   transtype = 'TTR'  
                  GROUP BY insurance_file_cnt ,risk_cnt  
                ) risk_taxes  
                 ON irl1.insurance_file_cnt =risk_taxes.insurance_file_cnt AND  irl1.risk_cnt = risk_taxes.risk_cnt  
     LEFT JOIN (SELECT  risk_cnt, sum(currency_amount) totalfees  
                FROM    policy_fee_u  
                WHERE   insurance_file_cnt = @insurance_file_cnt  
                GROUP BY risk_cnt  
                ) risk_fees  
                ON irl1.risk_cnt = risk_fees.risk_cnt  
        LEFT JOIN  
               (SELECT  risk_cnt, sum(this_premium) totallevytax  
                FROM    Peril  
      WHERE is_levy_tax=1 AND is_premium=0 AND is_taxed IS NULL  
                GROUP BY risk_cnt  
                ) levy_tax  
                ON irl1.risk_cnt = levy_tax.risk_cnt  
  
        LEFT JOIN  
               (SELECT  risk_cnt, sum(this_premium) totalstampinsurer  
                FROM    Peril p  
                JOIN   peril_type pt  
                       ON p.peril_type_id = pt.peril_type_id where isnull(pt.is_stamp_duty_insurer,0) = 1  
                GROUP BY p.risk_cnt ) stampinsurer  
                ON irl1.risk_cnt = stampinsurer.risk_cnt  
  
       LEFT JOIN  
               (SELECT  risk_cnt, sum(this_premium) totalstampinsured  
                FROM    Peril p  
                JOIN   peril_type pt  
                      ON p.peril_type_id = pt.peril_type_id where isnull(pt.is_stamp_duty_insured,0) = 1  
                GROUP BY p.risk_cnt  
                ) stampinsured  
                ON irl1.risk_cnt = stampinsured.risk_cnt  
  
   LEFT JOIN insurance_file_pt_log ifpt on ifpt.risk_cnt=rsk.risk_cnt  
   LEFT JOIN insurance_file_clone_log ifct on ifct.risk_cnt=rsk.risk_cnt  
  
  LEFT JOIN  #temp_Rating_Section RS ON RS.Risk_Cnt = RSK.Risk_Cnt  
  
       INNER JOIN insurance_file_risk_link [LastChangeVerionLink]  
       INNER JOIN insurance_file  [LastVersion]  
    ON [LastVersion].insurance_file_cnt = [LastChangeVerionLink].insurance_file_cnt  
    ON [LastChangeVerionLink].risk_cnt = irl1.risk_cnt AND ([LastChangeVerionLink].status_flag ='C' OR [LastChangeVerionLink].status_flag ='D')  
        WHERE   ifi.insurance_file_cnt = @insurance_file_cnt  
        AND     ISNULL(ifi.insurance_file_status_id, 2) IN (1,2,3,4,5,6,309,@void_Insurance_file_status_id)  
        ORDER BY rsk.risk_number  
  
DROP TABLE #InsuranceFileRiskLink  

If Object_ID('#temp_Rating_Section') Is not Null
	drop table #temp_Rating_Section