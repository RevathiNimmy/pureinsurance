SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_wp_policyfee'
GO


CREATE PROCEDURE spu_wp_policyfee   
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskID INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

DECLARE 
    @this_premium MONEY,
    @total_premium MONEY,
    @total_premium_inc_override MONEY,
    @total_discount MONEY,
    @total_extra MONEY,
    @total_extras_ipt_value MONEY,
    @total_fee MONEY,
    @total_insurer_fee MONEY,
    @isIPTable TINYINT,
    @ipt_rate FLOAT,
    @extras_ipt_value MONEY,
    @extras_incl_ipt MONEY,
    @subagent_value MONEY,
    @agent_value MONEY,
    @count INT,
    @extra_value MONEY,
    @extra_value1 MONEY,
    @extra_value2 MONEY,
    @name VARCHAR(60),
    @name1 VARCHAR(60),
    @name2 VARCHAR(60),
    @schemename VARCHAR(30),
    @schemename1 VARCHAR(30),
    @schemename2 VARCHAR(30),
    @currency_ISO_code VARCHAR(4),
    @currency_name VARCHAR(255),
    @currency_symbol VARCHAR(4),
    @override MONEY,
    @sql VARCHAR(8000),
    @datamodel VARCHAR(10),
    @Output integer,
    @TableName varchar(255),
    @FsaTypeOfSale varchar(100),
    @FsaTypeOfSale1 varchar(100),
    @FsaTypeOfSale2 varchar(100)

/*Get the value of the premium without the add ons and the currency details*/
SELECT 
    @this_premium = ROUND(ISNULL(i.this_premium,0),2),
    @currency_ISO_code = c.iso_code,
    @currency_name = c.description,
    @currency_symbol = c.symbol
FROM insurance_file i
JOIN currency c
    ON c.currency_id = i.currency_id
WHERE i.insurance_file_cnt = @InsuranceFileCnt

/*Get the total discount value of add ons*/
SELECT @total_discount = ISNULL(SUM(ISNULL(pf.fee_amount,0)),0)
FROM policy_fee pf
JOIN party p
    ON p.party_cnt = pf.party_cnt
JOIN party_type pt
    ON pt.party_type_id = p.party_type_id
WHERE pf.insurance_file_cnt = @InsuranceFileCnt
AND pt.code = 'DI'

/*Get the total extra value of add ons*/
SELECT @total_extra = ISNULL(SUM(ROUND(ISNULL(pf.fee_amount,0),2)),0)
FROM policy_fee pf
JOIN party p
    ON p.party_cnt = pf.party_cnt
JOIN party_type pt
    ON pt.party_type_id = p.party_type_id
WHERE pf.insurance_file_cnt = @InsuranceFileCnt
AND pt.code = 'EX'

/*Get the total extra ipt value of add ons*/
SELECT @total_extras_ipt_value = ISNULL(SUM(ROUND(ISNULL(pf.tax_amount,0),2)),0)
FROM policy_fee pf
JOIN party p
    ON p.party_cnt = pf.party_cnt
JOIN party_type pt
    ON pt.party_type_id = p.party_type_id
WHERE pf.insurance_file_cnt = @InsuranceFileCnt
AND pt.code = 'EX'

/*Get the total fee value of add ons*/
SELECT @total_fee = ISNULL(SUM(ISNULL(pf.fee_amount,0)),0)
FROM policy_fee pf
JOIN party p
    ON p.party_cnt = pf.party_cnt
JOIN party_type pt
    ON pt.party_type_id = p.party_type_id
WHERE pf.insurance_file_cnt = @InsuranceFileCnt
AND pt.code = 'FE'

/*Get the total insurer fee value of add ons*/
SELECT @total_insurer_fee = ISNULL(SUM(ISNULL(pf.fee_amount,0)),0)
FROM policy_fee pf
JOIN party p
    ON p.party_cnt = pf.party_cnt
JOIN party_type pt
    ON pt.party_type_id = p.party_type_id
WHERE pf.insurance_file_cnt = @InsuranceFileCnt
AND pt.code = 'IN'

/*Get subagent commission value*/
SELECT @subagent_value = ISNULL(SUM(ISNULL(pola.agent_commission_value,0)),0)
FROM policy_agents pola
JOIN party_agent pa
    ON pa.party_cnt = pola.agent_cnt
JOIN party_agent_type pat
    ON pat.party_agent_type_id = pa.party_agent_type_id
WHERE pola.insurance_file_cnt = @InsuranceFileCnt
AND pat.description = 'SUB AGENT'

/*Get agent commission value*/
SELECT @agent_value = ISNULL(SUM(ISNULL(pola.agent_commission_value,0)),0)
FROM policy_agents pola
JOIN party_agent pa
    ON pa.party_cnt = pola.agent_cnt
JOIN party_agent_type pat
    ON pat.party_agent_type_id = pa.party_agent_type_id
WHERE pola.insurance_file_cnt = @InsuranceFileCnt
AND pat.description = 'AGENT'

/*Get the IPT rate so we can calculate extra IPT amounts*/
SELECT
    @ipt_rate = ISNULL(MAX(ip.rate),0)
FROM ipt ip
JOIN insurance_file i
    ON ip.risk_code_id = i.risk_code_id
WHERE i.insurance_File_Cnt = @InsuranceFileCnt
AND ip.effective_date =
( 
    SELECT 
        MAX(effective_date)
    FROM ipt
    WHERE risk_code_id = i.risk_code_id
    AND effective_date < GETDATE()
)

/*Default the variables*/
SELECT @extras_ipt_value = 0
SELECT @count = 0
SELECT @name1 = ''
SELECT @name2 = ''
SELECT @extra_value1 = 0.00
SELECT @extra_value2 = 0.00
SELECT @schemename1 = ''
SELECT @schemename2 = ''

/*Cycle through each Extra*/
DECLARE c_cursor SCROLL CURSOR FOR
    SELECT
        p.name,
        ROUND(pf.fee_amount, 2),
        ROUND(pf.tax_amount, 2),
        ex.description,
        ftos.description
    FROM policy_fee pf
    JOIN party p 
        ON p.party_cnt = pf.party_cnt
    JOIN party_type pt 
        ON pt.party_type_id = p.party_type_id
    LEFT JOIN extra_scheme ex 
        ON ex.extra_scheme_id=pf.extra_scheme_id
    LEFT JOIN fsa_type_of_sale ftos
        ON pf.fsa_type_of_sale_id = ftos.fsa_type_of_sale_id        
    WHERE pf.insurance_file_cnt = @InsuranceFileCnt
    AND pt.code = 'EX'

OPEN c_cursor

FETCH NEXT FROM c_cursor INTO
    @name,
    @extra_value,
    @extras_ipt_value,
    @schemename,
    @FsaTypeOfSale

WHILE @@FETCH_STATUS = 0
BEGIN

    /*Get extra names and values*/
    SELECT @count = @count + 1

    IF @count = 1
    BEGIN
        SELECT
            @name1 = @name,
            @extra_value1 = @extra_value,
            @schemename1 = @schemename,
            @FsaTypeOfSale1 = @FsaTypeOfSale
    END

    IF @count = 2
    BEGIN
        SELECT
            @name2 = @name,
            @extra_value2 = @extra_value,
            @schemename2 = @schemename,
            @FsaTypeOfSale2 = @FsaTypeOfSale
    END

    FETCH NEXT FROM c_cursor INTO
        @name,
        @extra_value,
        @isIPTable,
        @schemename,
        @FsaTypeOfSale
END

CLOSE c_cursor
DEALLOCATE c_cursor

/*Get override amount*/

SELECT 
    @datamodel = RTRIM(gdm.code)
FROM risk r
JOIN gis_screen gs
    ON gs.gis_screen_id = r.gis_screen_id
JOIN gis_data_model gdm
    ON gdm.gis_data_model_id = gs.gis_data_model_id
WHERE r.risk_cnt = @RiskID

CREATE TABLE #TempOverride
(
    override MONEY
)

SELECT @sql =
    'INSERT INTO #TempOverride ' +
    'SELECT ' +
    '    o.override_amount ' + 
    'FROM gis_policy_link gpl ' +
    'JOIN ' + @datamodel + '_policy_binder pb ' +
    '    ON pb.gis_policy_link_id = gpl.gis_policy_link_id ' +
    'JOIN ' + @datamodel + '_output o ' +
    '    ON o.' + @datamodel + '_policy_binder_id = pb.' + @datamodel + '_policy_binder_id ' +
    'WHERE insurance_file_cnt = ' + CAST(@InsuranceFileCnt AS VARCHAR(20)) + ' ' +
    'AND risk_id = ' + CAST(@RiskID AS VARCHAR(20))

SELECT @TableName = @datamodel + '_output'
EXEC @Output = DDLExistsTable @TableName

IF @Output = 1
BEGIN
    EXEC @Output = DDLExistsColumn @TableName, 'override_amount'
END

IF @Output = 1
BEGIN
    EXEC (@sql)
END

SELECT 
    @override = ISNULL(SUM(override),0)
FROM #TempOverride

DROP TABLE #TempOverride

/*Calculate final amounts*/
SELECT @extras_incl_ipt = @total_extra + @total_extras_ipt_value
SELECT @total_premium = @this_premium + @total_fee + @extras_incl_ipt - @total_discount + @total_insurer_fee
SELECT @total_premium_inc_override = @total_premium + @override

IF @subagent_value <> 0
BEGIN
    SELECT @subagent_value = @total_premium - @subagent_value
END

IF NOT EXISTS(SELECT NULL FROM hidden_options WHERE option_number=61 AND branch_id=1)
BEGIN
    SET @FsaTypeOfSale1 =''
    SET @FsaTypeOfSale2 =''
END

/*Select the total value of the premium including add ons*/
SELECT 
    'total_premium' = @total_premium,
    'fees_value' = @total_fee, 
    'insurer_fee' = @total_insurer_fee,
    'extras_ipt_value' = @total_extras_ipt_value,
    'extras_incl_ipt' = @extras_incl_ipt,
    'discount_value' = @total_discount,
    'total_subagent_premium' = @subagent_value,
    'total_agent_premium' = @agent_value,
    'extra_value1' = @extra_value1,
    'extra_value2' = @extra_value2,
    'extra_name1' = @name1,
    'extra_name2' = @name2,
    'scheme_name1' = @schemename1,
    'scheme_name2' = @schemename2,
    'currency_ISO_code' = @currency_ISO_code,
    'currency_name' = @currency_name,
    'currency_symbol' = @currency_symbol,
    'total_premium_inc_override' = @total_premium_inc_override,
    'extra_FsaTypeOfSale1' = @FsaTypeOfSale1,
    'extra_FsaTypeOfSale2' = @FsaTypeOfSale2

GO
