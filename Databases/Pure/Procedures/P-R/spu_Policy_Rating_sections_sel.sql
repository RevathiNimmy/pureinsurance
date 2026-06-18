SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Policy_Rating_Section_sel'
GO

CREATE PROCEDURE spu_Policy_Rating_Section_sel
    @insuranceFileCnt int,
    @TransactionType int,
    @EffectiveDate datetime 
AS

DECLaRE @CommissionByAccountExec tinyint
DECLARE @CommissionCnt integer
DECLARE @CommissionAccount varchar(255)

DECLARE @InsurerCnt integer
DECLARE @RiskSectionId integer
DECLARE @RiskCodeId integer
DECLARE @RiskGroupId integer
DECLARE @SchemeId integer


DECLARE @Rate1 numeric(19,4) 
DECLARE @Value1 numeric(19,4)
DECLARE @MinimumTotal1 numeric(19,4)
DECLARE @Rate2 numeric(19,4) 
DECLARE @Value2 numeric(19,4)
DECLARE @MinimumTotal2 numeric(19,4)
DECLARE @Rate3 numeric(19,4) 
DECLARE @Value3 numeric(19,4)
DECLARE @MinimumTotal3 numeric(19,4)
DECLARE @CommissionTaxGroupId integer


CREATE TABLE #TempPolicySections
(
COB_Rating_Section_Id integer,
Section_Description varchar(255),
Insurance_Section_id integer,
Tax_group_id integer,
Premium_Excluding_tax numeric(19,4),
Tax_applied numeric(19,4),
Premium_Including_tax numeric(19,4),
Commission_cnt integer,
Commission_account varchar(255),
Commission_percentage numeric(19,4),
Commission_charge numeric(19,4),
Commission_net numeric(19,4),
Commission_tax_applied numeric(19,4),
Commission_payable numeric(19,4),
Commission_tax_group_id integer,
Is_minimum_brokerage tinyint,
Override_rate_table tinyint,
Multi_Insurer tinyint,
Single_Tax_Rate numeric(19,4),
Rate_Table_percentage numeric(19,4),
Rate_Table_charge numeric(19,4),
Rate_Table_Minimum numeric(19,4),
status smallint,
risk_code char(10),
placeholder1 tinyint, --used by bGISPMUExtras to store tax details
placeholder2 tinyint --used by bGISPMUExtras to store insurer details
)
INSERT INTO #TempPolicySections
  
SELECT
RTU.COB_Rating_Section_Id,
ISNULL(CRS.Description,'Not Applicable'),
ICS.insurance_section_id,
RTU.Tax_group_id,
ISNULL(ICS.Premium_Excluding_tax,0),
ISNULL(ICS.Tax_applied,0),
ISNULL(ICS.Premium_Including_tax,0),
0,
'',
ISNULL(ICS.commission_percentage,0),
ISNULL(ICS.commission_charge,0),
ISNULL(ICS.commission_net,0),
ISNULL(ICS.commission_tax_applied,0),
ISNULL(ICS.commission_payable,0),
ICS.commission_tax_group_id,
ISNULL(ICS.is_minimum_brokerage,0),
ISNULL(ICS.override_rate_table,0),
CASE PI.shortname
    WHEN 'MULTI' THEN 1
    ELSE 0
END,
0,
0,
0,
0,
CASE ISNULL(ICS.insurance_section_id,0)
WHEN 0 THEN 0
Else 1
END 'Status',
crs.code,
1,
1
FROM Risk_Tax_Usage RTU
JOIN Insurance_File I ON I.risk_code_id = RTU.risk_code_id
JOIN Party PI ON PI.party_cnt = I.lead_Insurer_cnt
LEFT OUTER JOIN COB_Rating_section CRS ON CRS.COB_Rating_Section_id = RTU.COB_Rating_Section_id
LEFT OUTER JOIN Insurance_COB_Section ICS ON ICS.insurance_file_Cnt = I.insurance_file_Cnt
                    AND IsNull(ICS.COB_Rating_Section_id,0) = IsNull(RTU.COB_Rating_Section_ID,0)
WHERE I.insurance_file_cnt = @InsuranceFileCnt

 

/*Get Single Tax Rate If possible*/

UPDATE  TPS 
SET TPS.Single_Tax_Rate = TBR.rate
              FROM Tax_Band_rate TBR 
              JOIN Tax_Group_Tax_Band TGTB ON TGTB.tax_band_id = TBR.Tax_Band_id
              JOIN Tax_Group TG ON TG.tax_group_id = TGTB.Tax_Group_id
              JOIN #TempPolicySections TPS ON TPS.tax_group_id = TG.tax_group_id
              WHERE  (SELECT MAX(sequence) FROM Tax_Group_Tax_Band
                    WHERE tax_group_id = TPS.tax_group_id) = 1
              AND TBR.is_value = 0
              AND TBR.effective_date <= @EffectiveDate
              AND ((@TransactionType = 1 AND TBR.NB = 1)
              OR  (@TransactionType = 2 AND TBR.REN = 1)
              OR  (@TransactionType = 3 AND TPS.Premium_Excluding_Tax > 0 AND TBR.AMTA = 1)
              OR  (@TransactionType = 3 AND TPS.Premium_Excluding_Tax < 0 AND TBR.RMTA = 1)
            ) 
 
/* Get Commission Details */
SELECT @InsurerCnt = 0
SELECT @CommissionCnt = 0
SELECT @RiskCodeId = 0
SELECT @RiskGroupId = 0
SELECT @SchemeId = 0
SELECT @InsurerCnt=I.lead_insurer_cnt,@SchemeId=I.scheme,@RiskGroupId =RC.risk_group_id,@RiskCodeId=I.risk_code_id  FROM risk_code RC
                        JOIN Insurance_File I ON I.risk_code_id = RC.risk_code_id
                        WHERE I.Insurance_File_Cnt = @InsuranceFileCnt 
 
/*Get Commission Account details and Update the Table */

SELECT @CommissionByAccountExec = ISNULL((SELECT Value from hidden_options
                        WHERE branch_id = 1
                        AND option_number = 40),0)

IF @CommissionByAccountExec = 0
    BEGIN
    SELECT @CommissionCnt = ISNULL((SELECT commission_cnt FROM risk_by_source RBS
              JOIN Risk_code RC ON RC.risk_group_id = RBS.risk_group_id
              JOIN Insurance_file I ON I.risk_code_id = RC.risk_code_id
                        AND I.source_id = RBS.source_id
              WHERE I.Insurance_file_cnt = @InsuranceFileCnt),0)
    IF @CommissionCnt = 0
    BEGIN
    SELECT @CommissionCnt = ISNULL((SELECT commission_cnt FROM risk_by_source RBS
              JOIN Risk_code RC ON RC.risk_group_id = RBS.risk_group_id
              JOIN Insurance_file I ON I.risk_code_id = RC.risk_code_id
                        AND RBS.source_id =0 
              WHERE I.Insurance_file_cnt = @InsuranceFileCnt),0)

    END
END
IF @CommissionByAccountExec = 1
BEGIN

    SELECT @CommissionCnt = ISNULL((SELECT DISTINCT PTY.party_cnt 
                FROM Party PTY
                JOIN Party_Consultant PTC
                    ON PTC.commission_cnt = PTY.party_cnt
                JOIN Insurance_file I 
                    ON I.account_executive_cnt = PTC.party_cnt 
              WHERE I.Insurance_file_cnt = @InsuranceFileCnt),0)

  
END
SELECT @CommissionAccount = (SELECT name FROM Party where party_cnt = @CommissionCnt)
UPDATE #TempPolicySections 
SET commission_cnt = @CommissionCnt,commission_account = @CommissionAccount


DECLARE C_Rating_Sections CURSOR FORWARD_ONLY STATIC FOR
Select COB_Rating_Section_id FROM #TempPolicySections
WHERE multi_insurer =0 
 
OPEN C_Rating_Sections
FETCH NEXT FROM C_Rating_Sections INTO @RiskSectionId 
WHILE @@FETCH_STATUS = 0
BEGIN
 
exec spu_get_commissionrates @insurerCnt,@SchemeId,@RiskGroupId,@RiskCodeId,@RiskSectionId,@EffectiveDate,
@Rate1 OUTPUT,@Value1 OUTPUT,@MinimumTotal1 OUTPUT,@Rate2 OUTPUT,@Value2 OUTPUT,@MinimumTotal2 OUTPUT,
@Rate3 OUTPUT,@Value3 OUTPUT,@MinimumTotal3 OUTPUT,@CommissionTaxGroupId OUTPUT 
/*No Rates Present */
 
IF @Rate1 is null
BEGIN
    UPDATE #TempPolicySections
    SET override_rate_table = 1
    WHERE COB_Rating_Section_Id =@RiskSectionId 
END
ELSE
BEGIN
    IF @TransactionType =  1
    BEGIN
  
        UPDATE #TempPolicySections
        set commission_tax_group_id = @CommissionTaxGroupId,
        rate_table_percentage = @Rate1,
        rate_table_charge = @Value1,
        rate_table_minimum = @MinimumTotal1
        WHERE ISNULL(COB_Rating_Section_Id,0) =ISNULL(@RiskSectionId,0)
    
        UPDATE #TempPolicySections
        set commission_tax_group_id = @CommissionTaxGroupId,
        commission_percentage = @Rate1,
        commission_charge = @Value1 
        WHERE ISNULL(COB_Rating_Section_Id,0) =ISNULL(@RiskSectionId,0)
        AND override_rate_table = 0
    
    END
    IF @TransactionType =  2
    BEGIN
    
        UPDATE #TempPolicySections
        SET commission_tax_group_id = @CommissionTaxGroupId,
        rate_table_percentage = @Rate2,
        rate_table_charge = @Value2,
        rate_table_minimum = @MinimumTotal2
        WHERE ISNULL(COB_Rating_Section_Id,0) =ISNULL(@RiskSectionId,0)
    
        UPDATE #TempPolicySections
        SET commission_tax_group_id = @CommissionTaxGroupId,
        commission_percentage = @Rate2,
        commission_charge = @Value2 
        WHERE ISNULL(COB_Rating_Section_Id,0) =ISNULL(@RiskSectionId,0)
        AND override_rate_table = 0
    END
    IF @TransactionType =  3
    BEGIN

        UPDATE #TempPolicySections
        SET commission_tax_group_id = @CommissionTaxGroupId,
        rate_table_percentage = @Rate3,
        rate_table_charge = @Value3,
        rate_table_minimum = @MinimumTotal3
        WHERE ISNULL(COB_Rating_Section_Id,0) =ISNULL(@RiskSectionId,0)

        UPDATE #TempPolicySections
        SET commission_tax_group_id = @CommissionTaxGroupId,
        commission_percentage = @Rate3,
        commission_charge = @Value3 
        WHERE ISNULL(COB_Rating_Section_Id,0) =ISNULL(@RiskSectionId,0)
        AND override_rate_table = 0
    END
END
  
UPDATE #TempPolicySections
    SET commission_net = (premium_excluding_tax * commission_percentage / 100)
                + commission_charge 
    WHERE ISNULL(COB_Rating_Section_Id,0) =ISNULL(@RiskSectionId,0)
    AND premium_excluding_tax <> 0
     

UPDATE #TempPolicySections
    SET commission_net = rate_table_minimum,
    is_minimum_brokerage = 1
    WHERE ISNULL(COB_Rating_Section_Id,0) =ISNULL(@RiskSectionId,0)
    AND ABS(commission_net) < ABS(rate_table_minimum)
    AND rate_table_minimum <> 0
    AND override_rate_table = 0
 
UPDATE #TempPolicySections
    SET commission_payable = commission_net + commission_tax_applied
    WHERE ISNULL(COB_Rating_Section_Id,0) =ISNULL(@RiskSectionId,0) 
 
 
FETCH NEXT FROM C_Rating_Sections INTO @RiskSectionId
END 

CLOSE C_Rating_Sections
DEALLOCATE C_Rating_Sections

SELECT * FROM #TempPolicySections

DROP TABLE #TempPolicySections 
GO

 
