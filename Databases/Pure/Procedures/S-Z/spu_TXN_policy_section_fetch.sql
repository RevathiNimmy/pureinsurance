SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_TXN_policy_section_fetch'
GO

CREATE PROCEDURE spu_TXN_policy_section_fetch
(
@from_event bit,
@insurance_file_cnt int,
@RiskGroupId int,
@CountryId int,
@TransactionType int,
@EffectiveDate datetime,
@From_BreakDown BIT =0 
)

AS

DECLARE @risk_code_id int
DECLARE @iCOBRatingSectionId int
DECLARE @iTaxGroupId int
DECLARE @vTaxGroupCode varchar(10)
DECLARE @fTaxRate float
DECLARE @icalcBasis int
DECLARE @iTaxEditable int
DECLARE @insurer_id int
DECLARE @nCommissionPercent numeric(7,4)
DECLARE @nCommissionAmount numeric(19,4)
DECLARE @nMinimumBrokerage numeric(19,4)
DECLARE @iTGId int
Declare @Gis_Scheme_Id int
Declare @Real_Insurance_File_Cnt int

CREATE TABLE #Policy_Section
(
    insurance_section_id int,
    insurance_file_cnt int,
    COB_rating_section_id int,
    description varchar(255) null,
    is_levy_section bit,
    extra bit,
    premium_excluding_tax numeric(19,4),
    tax_group_id int,
    code varchar(255) null,
    percentage1 float,
    value money,
    premium_including_tax numeric(19,4),
    commission_percentage numeric(19,4),
    commission_charge numeric(19,4),
    commission_net numeric(19,4),
    commission_tax_group_id int,
    commission_code varchar(255) null,
    percentage2 float,
    value1 money,
    commission_payable numeric(19,4),
    extra1 bit,
    calc_basis int,
    commission_percentage_default numeric(19,4),
    commission_charge_default numeric(19,4),
    commission_min_default numeric(19,4),
    override_rate_table tinyint,
    tax_editable tinyint,
    sequence varchar(255)
)

IF @from_event=0
BEGIN

    SELECT @risk_code_id=risk_code_id,@insurer_id=lead_insurer_cnt FROM insurance_file WHERE insurance_file_cnt=@insurance_file_cnt


    INSERT INTO #Policy_Section
    SELECT
    0,
    0,
    CRS.COB_rating_section_id,
    CRS.description,
    CRS.is_levy_section,
    1,
    0,
    0,
    '',
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    '',
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    RTU.sequence
    FROM
    Risk_Tax_Usage RTU
    INNER JOIN COB_rating_section CRS ON RTU.COB_rating_section_id=CRS.COB_rating_section_id
    WHERE RTU.risk_code_id=@risk_code_id
    AND CRS.COB_rating_section_id NOT IN (SELECT COB_rating_section_id FROM insurance_COB_section ICS WHERE ICS.insurance_file_cnt=@insurance_file_cnt)

    DECLARE TXN_SECTION_CURSOR CURSOR FOR
    SELECT
    COB_rating_section_id
    FROM
    #Policy_section

    OPEN TXN_SECTION_CURSOR

    FETCH NEXT FROM TXN_SECTION_CURSOR
    INTO @iCOBRatingSectionId

    WHILE @@FETCH_STATUS = 0
    BEGIN

        EXECUTE spu_TXN_section_tax_rate_sel
        NULL,
        0,
        @RiskGroupId,
        @risk_code_id,
        @iCOBRatingSectionId,
        @CountryId,
        @TransactionType,
        @EffectiveDate,
        @insurer_id,
        @rTaxGroupId= @iTaxGroupId OUTPUT,
        @rTaxGroupCode= @vTaxGroupCode OUTPUT,
        @rTaxRate= @fTaxRate OUTPUT,
        @rcalcBasis=@icalcBasis OUTPUT,
        @rTaxEditable=@iTaxEditable OUTPUT

        UPDATE #Policy_Section
        SET
        tax_group_id = @iTaxGroupId,
        code = @vTaxGroupCode,
        percentage1 = @fTaxRate/100,
        calc_basis=@icalcBasis,
        tax_editable=@iTaxEditable
        where
        COB_rating_section_id=@iCOBRatingSectionId

        EXECUTE spu_TXN_insurer_commission_rate_sel
        @insurer_id,
        @RiskGroupId,
        @risk_code_id,
        @iCOBRatingSectionId,
        @TransactionType,
        @EffectiveDate,
        @rCommissionPercent=@nCommissionPercent OUTPUT,
        @rCommissionAmount=@nCommissionAmount OUTPUT,
        @rMinimumBrokerage=@nMinimumBrokerage OUTPUT,
        @rTaxGroupId=@iTaxGroupId OUTPUT

        UPDATE #Policy_Section
        SET
        commission_percentage = ISNULL(@nCommissionPercent, 0)/100
        WHERE
        COB_rating_section_id=@iCOBRatingSectionId      

        EXECUTE spu_TXN_section_tax_rate_sel
        @iTaxGroupId,
        1,
        @RiskGroupId,
        @risk_code_id,
        @iCOBRatingSectionId,
        @CountryId,
        @TransactionType,
        @EffectiveDate,
        @insurer_id,
        @rTaxGroupId=@iTGId OUTPUT,
        @rTaxGroupCode=@vTaxGroupCode OUTPUT,
        @rTaxRate=@fTaxRate OUTPUT,
        @rcalcBasis=@icalcBasis OUTPUT,
        @rTaxEditable=@iTaxEditable OUTPUT

        UPDATE #Policy_Section
        SET
        commission_tax_group_id=@iTGId,
        commission_code=@vTaxGroupCode,
        percentage2=@fTaxRate/100
        WHERE
        COB_rating_section_id=@iCOBRatingSectionId

        FETCH NEXT FROM TXN_SECTION_CURSOR
        INTO @iCOBRatingSectionId
    END

        CLOSE TXN_SECTION_CURSOR
        DEALLOCATE TXN_SECTION_CURSOR

        INSERT INTO #Policy_Section
        SELECT
        ICS.insurance_section_id,
        @insurance_file_cnt,
        CRS.COB_rating_section_id,
        CRS.description,
        ISNULL(CRS.is_levy_section,0),
        ICS.IS_APPLIED,
        ICS.premium_excluding_tax,
        ICS.tax_group_id,
        TG1.code,
        TC1.percentage/100,
        TC1.value,
        ICS.premium_including_tax,
        ICS.commission_percentage/100,
        ICS.commission_charge,
        ICS.commission_net,
        ICS.commission_tax_group_id,
        TG2.code AS commission_code,
        TC2.percentage/100,
        TC2.value,
        ICS.commission_payable,
        0,
        ISNULL(TC2.calc_basis,0) AS calc_basis,
        0,
        0,
        0,
        ICS.override_rate_table,
        TG1.is_tax_amount_editable,
        RTU.sequence
        FROM
        insurance_COB_section ICS
        INNER JOIN COB_rating_section CRS ON CRS.COB_rating_section_id=ICS.COB_rating_section_id
        LEFT OUTER JOIN tax_group TG1 ON TG1.tax_group_id=ICS.tax_group_id
        LEFT OUTER JOIN tax_group TG2 ON TG2.tax_group_id=ICS.commission_tax_group_id
        LEFT OUTER JOIN tax_calculation TC1 ON TC1.insurance_file_cnt=@insurance_file_cnt AND TC1.insurance_section_id=ICS.insurance_section_id AND TC1.is_commission_tax=0
        LEFT OUTER JOIN tax_calculation TC2 ON TC2.insurance_file_cnt=@insurance_file_cnt AND TC2.insurance_section_id=ICS.insurance_section_id AND TC2.is_commission_tax=1
        LEFT JOIN risk_tax_usage rtu ON rtu.COB_rating_section_id = ICS.COB_rating_section_id
        AND rtu.risk_code_id =@risk_code_id
        WHERE
        ICS.insurance_file_cnt=@insurance_file_cnt



END

ELSE

BEGIN

    SELECT @risk_code_id=risk_code_id,@insurer_id=lead_insurer_cnt FROM event_insurance_file WHERE insurance_file_cnt=@insurance_file_cnt
    
    INSERT INTO #Policy_Section
    SELECT
    0,
    0,
    CRS.COB_rating_section_id,
    CRS.description,
    CRS.is_levy_section,
    0,
    0,
    0,
    '',
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    '',
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    RTU.sequence
    FROM
    Risk_Tax_Usage RTU
    INNER JOIN COB_rating_section CRS ON RTU.COB_rating_section_id=CRS.COB_rating_section_id
    WHERE RTU.risk_code_id=@risk_code_id
    AND CRS.COB_rating_section_id NOT IN (SELECT COB_rating_section_id FROM event_insurance_COB_section ICS WHERE ICS.insurance_file_cnt=@insurance_file_cnt)

    DECLARE TXN_SECTION_CURSOR CURSOR FOR
    SELECT
    COB_rating_section_id
    FROM
    #Policy_section

    OPEN TXN_SECTION_CURSOR

    FETCH NEXT FROM TXN_SECTION_CURSOR
    INTO @iCOBRatingSectionId

    WHILE @@FETCH_STATUS = 0
    BEGIN

        EXECUTE spu_TXN_section_tax_rate_sel
        NULL,
        0,
        @RiskGroupId,
        @risk_code_id,
        @iCOBRatingSectionId,
        @CountryId,
        @TransactionType,
        @EffectiveDate,
        @insurer_id,
        @rTaxGroupId= @iTaxGroupId OUTPUT,
        @rTaxGroupCode= @vTaxGroupCode OUTPUT,
        @rTaxRate= @fTaxRate OUTPUT,
        @rcalcBasis=@icalcBasis OUTPUT,
        @rTaxEditable=@iTaxEditable OUTPUT

        UPDATE #Policy_Section
        SET
        tax_group_id = @iTaxGroupId,
        code = @vTaxGroupCode,
        percentage1 = @fTaxRate/100,
        calc_basis=@icalcBasis,
        tax_editable=@iTaxEditable
        where
        COB_rating_section_id=@iCOBRatingSectionId

        EXECUTE spu_TXN_insurer_commission_rate_sel
        @insurer_id,
        @RiskGroupId,
        @risk_code_id,
        @iCOBRatingSectionId,
        @TransactionType,
        @EffectiveDate,
        @rCommissionPercent=@nCommissionPercent OUTPUT,
        @rCommissionAmount=@nCommissionAmount OUTPUT,
        @rMinimumBrokerage=@nMinimumBrokerage OUTPUT,
        @rTaxGroupId=@iTaxGroupId OUTPUT

        UPDATE #Policy_Section
        SET
        commission_percentage = ISNULL(@nCommissionPercent, 0)/100
        WHERE
        COB_rating_section_id=@iCOBRatingSectionId      

        EXECUTE spu_TXN_section_tax_rate_sel
        @iTaxGroupId,
        1,
        @RiskGroupId,
        @risk_code_id,
        @iCOBRatingSectionId,
        @CountryId,
        @TransactionType,
        @EffectiveDate,
        @insurer_id,
        @rTaxGroupId=@iTGId OUTPUT,
        @rTaxGroupCode=@vTaxGroupCode OUTPUT,
        @rTaxRate=@fTaxRate OUTPUT,
        @rcalcBasis=@icalcBasis OUTPUT,
        @rTaxEditable=@iTaxEditable OUTPUT

        UPDATE #Policy_Section
        SET
        commission_tax_group_id=@iTGId,
        commission_code=@vTaxGroupCode,
        percentage2=@fTaxRate/100
        WHERE
        COB_rating_section_id=@iCOBRatingSectionId

        FETCH NEXT FROM TXN_SECTION_CURSOR
        INTO @iCOBRatingSectionId
    END

        CLOSE TXN_SECTION_CURSOR
        DEALLOCATE TXN_SECTION_CURSOR

        INSERT INTO #Policy_Section
        SELECT
        ICS.insurance_section_id,
        @insurance_file_cnt,
        CRS.COB_rating_section_id,
        CRS.description,
        ISNULL(CRS.is_levy_section,0),
        ICS.IS_APPLIED,
        ICS.premium_excluding_tax,
        ICS.tax_group_id,
        TG1.code,
        TC1.percentage/100,
        TC1.value,
        ICS.premium_including_tax,
        ICS.commission_percentage/100,
        ICS.commission_charge,
        ICS.commission_net,
        ICS.commission_tax_group_id,
        TG2.code AS commission_code,
        TC2.percentage/100,
        TC2.value,
        ICS.commission_payable,
        0,
        ISNULL(TC2.calc_basis,0) AS calc_basis,
        0,
        0,
        0,
        ICS.override_rate_table,
        TG1.is_tax_amount_editable,
        RTU.sequence
        FROM
        event_insurance_COB_section ICS
        INNER JOIN COB_rating_section CRS ON CRS.COB_rating_section_id=ICS.COB_rating_section_id
        LEFT OUTER JOIN tax_group TG1 ON TG1.tax_group_id=ICS.tax_group_id
        LEFT OUTER JOIN tax_group TG2 ON TG2.tax_group_id=ICS.commission_tax_group_id
        LEFT OUTER JOIN event_tax_calculation TC1 ON TC1.insurance_file_cnt=@insurance_file_cnt AND TC1.insurance_section_id=ICS.insurance_section_id AND TC1.is_commission_tax=0
        LEFT OUTER JOIN event_tax_calculation TC2 ON TC2.insurance_file_cnt=@insurance_file_cnt AND TC2.insurance_section_id=ICS.insurance_section_id AND TC2.is_commission_tax=1
        LEFT JOIN risk_tax_usage rtu ON rtu.COB_rating_section_id = ICS.COB_rating_section_id
        AND rtu.risk_code_id =@risk_code_id
        WHERE
        ICS.insurance_file_cnt=@insurance_file_cnt


END

    DECLARE TXN_SECTION_CURSOR CURSOR FOR
    SELECT
    COB_rating_section_id
    FROM
    #Policy_section

    OPEN TXN_SECTION_CURSOR

    FETCH NEXT FROM TXN_SECTION_CURSOR
    INTO @iCOBRatingSectionId

    WHILE @@FETCH_STATUS = 0
    BEGIN
        if @From_Event = 1
        Begin
            select @Real_Insurance_File_Cnt = max(insurance_file_cnt) from event_log where event_cnt = @insurance_file_cnt
        End
        Else
        Begin
            select @Real_Insurance_File_Cnt = @insurance_file_cnt
        End 
        select @Gis_Scheme_Id = gis_scheme_id from gis_policy_link where insurance_file_cnt=@Real_Insurance_File_Cnt
        
        EXECUTE spu_TXN_section_tax_rate_sel  
        NULL,  
        0,  
        @RiskGroupId,  
        @risk_code_id,  
        @iCOBRatingSectionId,  
        @CountryId,  
        @TransactionType,  
        @EffectiveDate,  
        @insurer_id,  
        @rTaxGroupId= @iTaxGroupId OUTPUT,  
        @rTaxGroupCode= @vTaxGroupCode OUTPUT,  
        @rTaxRate= @fTaxRate OUTPUT,  
        @rcalcBasis=@icalcBasis OUTPUT,
        @rTaxEditable=@iTaxEditable OUTPUT

        IF (SELECT code FROM #Policy_Section WHERE COB_rating_section_id=@iCOBRatingSectionId) IS NULL 
        BEGIN
           UPDATE #Policy_Section  
           SET  
             tax_group_id = @iTaxGroupId,  
             code = @vTaxGroupCode,
             tax_editable = @iTaxEditable
           where  
             COB_rating_section_id=@iCOBRatingSectionId  
        END
        
        EXECUTE spu_TXN_insurer_commission_rate_sel
        @insurer_id,
        @RiskGroupId,
        @risk_code_id,
        @iCOBRatingSectionId,
        @TransactionType,
        @EffectiveDate,
        @rCommissionPercent=@nCommissionPercent OUTPUT,
        @rCommissionAmount=@nCommissionAmount OUTPUT,
        @rMinimumBrokerage=@nMinimumBrokerage OUTPUT,
        @rTaxGroupId=@iTaxGroupId OUTPUT,
        @GisSchemeId=@Gis_Scheme_Id

        UPDATE #Policy_Section
        SET
        commission_percentage_default=@nCommissionPercent/100,
        commission_charge_default=@nCommissionAmount,
        commission_min_default = @nMinimumBrokerage
        WHERE
        COB_rating_section_id=@iCOBRatingSectionId

        UPDATE #Policy_Section
        SET
        commission_percentage=ISNULL(@nCommissionPercent, 0)/100,
        commission_charge=ISNULL(@nCommissionAmount, 0)
        WHERE
        COB_rating_section_id=@iCOBRatingSectionId
        AND override_rate_table=0
        AND (@from_event<>0 OR insurance_section_id=0)
        AND @From_BreakDown =0
        
        FETCH NEXT FROM TXN_SECTION_CURSOR
        INTO @iCOBRatingSectionId
    END

    CLOSE TXN_SECTION_CURSOR
    DEALLOCATE TXN_SECTION_CURSOR


UPDATE #POLICY_SECTION SET [sequence] = 'Not Selected' WHERE [sequence] = NULL

SELECT * FROM #POLICY_SECTION ORDER BY [sequence]
DROP TABLE #POLICY_SECTION


GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
