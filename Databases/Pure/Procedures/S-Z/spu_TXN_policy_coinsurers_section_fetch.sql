SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_TXN_policy_coinsurers_section_fetch'
GO

CREATE  PROCEDURE spu_TXN_policy_coinsurers_section_fetch
(
    @from_event BIT,
    @Insurance_File_Cnt INT,
    @transaction_type int = 0
)

AS

DECLARE @policy_coinsurers_section_id int
DECLARE @COB_rating_section_id int
DECLARE @insurer_id int
DECLARE @risk_code_id int
DECLARE @risk_group_id int
DECLARE @effective_date datetime

DECLARE @nCommissionPercent numeric(7,4)
DECLARE @nCommissionAmount numeric(19,4)
DECLARE @nMinimumBrokerage numeric(19,4)
DECLARE @iTaxGroupId int

DECLARE @nWrittenPercentage NUMERIC(19,4)

CREATE TABLE #Policy_Coinsurers_Section
(
    policy_coinsurers_section_id int,
    insurance_file_cnt int,
    party_cnt int,
    party_name varchar(255),
    COB_rating_section_id int,
    description varchar(255) null,
    is_levy_section bit,
    section_applied bit,
    applied bit,
    share_percent numeric(7,4),
    premium_inc_tax numeric(19,4),
    tax_group_id int,
    tax_group_code varchar(255),
    tax_percentage1 float,
    tax_value money,
    premium_exc_tax numeric(19,4),
    commission_percent numeric(7,4),
    commission_charge numeric(19,4),
    commission_exc_tax numeric(19,4),
    commission_tax_group_id int,
    commission_tax_group_code varchar(255),
    commission_tax_percent numeric(7,4),
    commission_tax numeric(19,4),
    commission_inc_tax numeric(19,4),
    persist_state int,
    calc_basis int,
    default_commission_percent numeric(7,4),
    default_commission_charge numeric(19,4),
    default_commission_minimum numeric(19,4),
    override_rate_table tinyint,
    tax_editable tinyint,
    written_line_percentage NUMERIC(19,4)
)

IF @from_event = 0
BEGIN
	
    --As written line percentage will be same for all the sections so its ok to get the value of last record.
    SELECT @nWrittenPercentage = written_line_percentage FROM policy_coinsurers WHERE insurance_file_cnt=@insurance_file_cnt
    
    SELECT @risk_code_id=INF.risk_code_id, @effective_date=INF.cover_start_date, @risk_group_id=RC.risk_group_id FROM
    insurance_file INF
    INNER JOIN party P ON INF.lead_insurer_cnt=P.party_cnt
    INNER JOIN risk_code RC ON RC.risk_code_id=INF.risk_code_id
    WHERE INF.insurance_file_cnt=@insurance_file_cnt

    INSERT INTO #Policy_Coinsurers_Section
    SELECT  
        pcs.policy_coinsurers_section_id,
        pcs.insurance_file_cnt,
        pcs.party_cnt,
        p.name,
        pcs.cob_rating_section_id,
        crs.description,
        crs.is_levy_section,
        1,
        pcs.is_applied,
        pcs.share_percent / 100,
        pcs.premium_inc_tax,
        pcs.tax_group_id,
        tg1.code,
        ISNULL(tc1.percentage, 0) / 100,
        ISNULL(tc1.value, 0),
        pcs.premium_exc_tax,
        pcs.commission_percent / 100,
        pcs.commission_charge,
        pcs.commission_exc_tax,
        pcs.commission_tax_group_id,
        ISNULL(tg2.code, ''),
        ISNULL(tc.percentage, 0) / 100,
        ISNULL(tc.value, 0),
        ISNULL(pcs.commission_inc_tax, 0),
        0,
        ISNULL(tc.calc_basis, 0),
        0,
        0,
        0,
        pcs.override_rate_table,
        tg1.is_tax_amount_editable,
        ISNULL(@nWrittenPercentage,0)/100
    FROM policy_coinsurers_section pcs
    JOIN party p 
        ON p.party_cnt = pcs.party_cnt
    JOIN cob_rating_section crs 
        ON crs.cob_rating_section_id = pcs.COB_rating_section_id 
    LEFT JOIN tax_calculation tc1 
        ON tc1.policy_coinsurers_section_id = pcs.policy_coinsurers_section_id 
        AND tc1.is_commission_tax = 0
    LEFT JOIN tax_group tg1 
        ON tg1.tax_group_id = pcs.tax_group_id 
    LEFT JOIN tax_group tg2 
        ON tg2.tax_group_id = pcs.commission_tax_group_id
    LEFT JOIN tax_calculation tc 
        ON tc.policy_coinsurers_section_id = pcs.policy_coinsurers_section_id 
        AND tc.is_commission_tax = 1
    WHERE pcs.insurance_file_cnt = @Insurance_File_Cnt
    ORDER BY pcs.party_cnt ASC
    
END
ELSE
BEGIN
	
    SELECT @nWrittenPercentage = written_line_percentage FROM event_policy_coinsurers WHERE insurance_file_cnt=@insurance_file_cnt
	
    SELECT @risk_code_id=INF.risk_code_id, @effective_date=INF.cover_start_date, @risk_group_id=RC.risk_group_id FROM
    event_insurance_file INF
    INNER JOIN party P ON INF.lead_insurer_cnt=P.party_cnt
    INNER JOIN risk_code RC ON RC.risk_code_id=INF.risk_code_id
    WHERE INF.insurance_file_cnt=@insurance_file_cnt

    INSERT INTO #Policy_Coinsurers_Section
    SELECT  
        pcs.policy_coinsurers_section_id,
        pcs.insurance_file_cnt,
        pcs.party_cnt,
        p.name,
        pcs.cob_rating_section_id,
        crs.description,
        crs.is_levy_section,
        1,
        pcs.is_applied,
        pcs.share_percent / 100,
        pcs.premium_inc_tax,
        pcs.tax_group_id,
        tg1.code,
        ISNULL(tc1.percentage, 0) / 100,
        ISNULL(tc1.value,0),
        pcs.premium_exc_tax,
        pcs.commission_percent / 100,
        pcs.commission_charge,
        pcs.commission_exc_tax,
        pcs.commission_tax_group_id,
        ISNULL(tg2.code, ''),
        ISNULL(tc.percentage, 0) / 100,
        ISNULL(tc.value, 0),
        ISNULL(pcs.commission_inc_tax, 0),
        0,
        ISNULL(tc.calc_basis, 0),
        0,
        0,
        0,
        pcs.override_rate_table,
        tg1.is_tax_amount_editable,
        ISNULL(@nWrittenPercentage,0)/100
    FROM event_policy_coinsurers_section pcs
    JOIN party p 
        ON p.party_cnt = pcs.party_cnt
    JOIN cob_rating_section crs 
        ON crs.cob_rating_section_id = pcs.cob_rating_section_id 
    LEFT JOIN event_tax_calculation tc1 
        ON tc1.policy_coinsurers_section_id = pcs.policy_coinsurers_section_id 
        AND tc1.is_commission_tax = 0
    LEFT JOIN tax_group tg1 
        ON tg1.tax_group_id = pcs.tax_group_id 
    LEFT JOIN tax_group tg2 
        ON tg2.tax_group_id = pcs.commission_tax_group_id 
    LEFT JOIN event_tax_calculation tc 
        ON tc.policy_coinsurers_section_id = pcs.policy_coinsurers_section_id 
        AND tc.is_commission_tax = 1
    WHERE pcs.insurance_file_cnt = @Insurance_File_Cnt
    ORDER BY pcs.party_cnt ASC

END

DECLARE TXN_SECTION_CURSOR CURSOR FOR
    SELECT
    policy_coinsurers_section_id, COB_rating_section_id, party_cnt 
    FROM
    #Policy_Coinsurers_Section

OPEN TXN_SECTION_CURSOR

FETCH NEXT FROM TXN_SECTION_CURSOR INTO @policy_coinsurers_section_id, @COB_rating_section_id, @insurer_id

WHILE @@FETCH_STATUS = 0
BEGIN
    EXECUTE spu_TXN_insurer_commission_rate_sel
    @insurer_id,
    @risk_group_id,
    @risk_code_id,
    @COB_rating_section_id,
    @transaction_type,
    @effective_date,
    @rCommissionPercent=@nCommissionPercent OUTPUT,
    @rCommissionAmount=@nCommissionAmount OUTPUT,
    @rMinimumBrokerage=@nMinimumBrokerage OUTPUT,
    @rTaxGroupId=@iTaxGroupId OUTPUT

    UPDATE #Policy_Coinsurers_Section
    SET
    default_commission_percent=@nCommissionPercent/100,
    default_commission_charge=@nCommissionAmount,
    default_commission_minimum = @nMinimumBrokerage
    WHERE
    policy_coinsurers_section_id=@policy_coinsurers_section_id

    UPDATE #Policy_Coinsurers_Section
    SET
    commission_percent=ISNULL(@nCommissionPercent, 0) /100,
    commission_charge=ISNULL(@nCommissionAmount, 0)
    WHERE
    policy_coinsurers_section_id=@policy_coinsurers_section_id
    AND override_rate_table=0
    AND @from_event<>0

    FETCH NEXT FROM TXN_SECTION_CURSOR INTO @policy_coinsurers_section_id, @COB_rating_section_id, @insurer_id

END

CLOSE TXN_SECTION_CURSOR
DEALLOCATE TXN_SECTION_CURSOR

SELECT * FROM #Policy_Coinsurers_Section
DROP TABLE #Policy_Coinsurers_Section

GO
