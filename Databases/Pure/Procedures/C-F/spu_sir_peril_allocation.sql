SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_sir_peril_allocation'
GO

CREATE PROCEDURE spu_sir_peril_allocation
    @rating_section_type_id int,
    @policy_section_type_id int,
    @insurance_file_cnt int,
    @risk_id int,
    @sum_insured numeric(19,4),
    @annual_rate numeric(19,4),
    @annual_premium numeric(19,4),
    @this_premium numeric(19,4),
    @rate_type_id int,
    @insurance_file_no_of_dp smallint,
    @original_flag smallint,
    @currency_id smallint,
    @country_id int,
    @state_id int,
    @is_amended tinyint = 0,
    @calculated_premium money = NULL,
    @override_reason varchar(255) = NULL,   
    @override_rating_section_create int = 0,   
    @override_rating_section_id int = 0,
    @auto_calculated tinyint=0,
    @earning_pattern_id int = 1
AS

/********************************************************************************************************/
/* Stored Procedure spu_peril_allocation                                     */
/********************************************************************************************************/
/* Revision             Description of Modification                                     Date        Who */
/* --------             ---------------------------                                     ----        --- */
/* 1.0                  TF27101997 - Created                                */
/* 1.1          TF11111997 - spu_pm_get_eff_id_from_code replaces spu_sir_get_id_from_code    */
/* 1.2                  RC15051998 - Amended for removal of R/I and new Peril layout re M4V4.           */
/* 1.3                  SR26092000 - Added rate_type_id as a new parameter since RSA system allows that */
/*                   to be over-ride                            */
/* 1.4                  SR26102000 - Getting is_premium, is_sum_insured , ri_band and xl_band from peril type and adding them to each peril */
/* 1.5                  Tomo30052002 - We don't want to recalculate the rating sections and perils when
                                       they're from the original risk, we want the actual ones reversed */
/* 1.6                  Thinh Nguyen 12/08/2002 - add in levy tax                                       */
/* 1.7                  RKS 14/10/2005 Premium Override work */
/********************************************************************************************************/

/*

Example:

Old risk has 5 rating sections, 2 old, 3 new
RSid    Code    SI    Prem    Orig
1       RAT1    0     -100    1
2       RAT2    0     -150    1
3       RAT1    1000   200    0
4       RAT2    2000   300    0
5       RAT3    3000   400    0

We MTA it, now get 7 rating sections, 3 old, 4 new
RSid    Code    SI    Prem    Orig
1       RAT1    0     - 50    1
2       RAT2    0     -100    1
3       RAT3    0     -200    1
4       RAT1    1000   200    0
5       RAT2    2000   300    0
6       RAT3    3000   400    0
7       RAT4    3000   400    0

The originals will _always_ be in the same order as on the previous risk, and will _always_ be first.
*/

    SET NOCOUNT ON

    /* Declare variables for use in processing */
    DECLARE 
        @effective_date     datetime,
        @id         int,
        @peril_group_id     int,
        @peril_count        smallint,
        @rating_section_id  int,
        @peril_type_id      int,
        @allocate_percent   numeric(12,8),
        @peril_id       int,
        @class_of_business_id   int,
        @final_annual_rate  numeric(21,6),
        @final_annual_premium   numeric(21,6),
        @final_this_premium numeric(21,6),
        @peril_sum_insured      numeric(21,6),
        @is_sum_insured     tinyint,
        @lead_commission_band   int,
        @sub_commission_band    int,
        @tax_group       int,
        @total_perils       numeric(19,4),
        @is_premium         tinyint,
        @ri_band        int,
        @xl_band        tinyint,
        @is_levy_tax    tinyint, -- Thinh Nguyen 12/08/2002
        @rowcount	int  

DECLARE @original_risk_cnt INT

DECLARE @offset INT,
        @old_premium numeric(19, 4),
        @new_premium numeric(19, 4),
        @factor float

    /* Set effective_date */
    SELECT  @effective_date = GetDate()

IF @insurance_file_no_of_dp IS NULL
SELECT @insurance_file_no_of_dp=2


IF ISNULL(@override_rating_section_create,0)= 0   
BEGIN  
    /* SEt the Policysection type to NULL if a negative value is passed */
    if (@policy_section_type_id < 0 )
        select @policy_section_type_id = NULL

    /* Get next rating_section_id */
    SELECT  @rating_section_id = MAX(rating_section_id) + 1
    FROM    Rating_Section
    WHERE   risk_cnt = @risk_id

    IF ISNULL(@rating_section_id, 0) = 0
        SELECT  @rating_section_id = 1

    /* Add Rating_Section */
    INSERT INTO Rating_Section
    (
        risk_cnt,
        rating_section_id,
        rating_section_type_id,
        policy_section_type_id,
        sequence_number,
        description,
        rate_type_id,
        annual_rate,
        sum_insured,
        annual_premium,
        this_premium,
        original_flag,
        currency_id,
        country_id,
        state_id,
        is_amended,
        calculated_premium,
        override_reason,
        auto_calculated,
        Earning_Pattern_id
    )
    VALUES
    (
        @risk_id,
        @rating_section_id,
        @rating_section_type_id,
        @policy_section_type_id,
        @rating_section_id,
        NULL,
        @rate_type_id,
        @annual_rate,
        @sum_insured,
        @annual_premium,
        @this_premium,
        @original_flag,
        @currency_id,
        @country_id,
        @state_id,
        @is_amended,
        CASE WHEN @calculated_premium=0 THEN @this_premium ELSE @calculated_premium END,
        @override_reason,
	@auto_calculated,
	@earning_pattern_id
    )
END   
ELSE  
 SET @rating_section_id = @override_rating_section_id  

IF @original_flag = 1
BEGIN

    SELECT @original_risk_cnt = original_risk_cnt
    FROM   insurance_file_risk_link WITH(NOLOCK)
    WHERE  insurance_file_cnt = @insurance_file_cnt
    AND    risk_cnt = @risk_id

    -- Where do the current rating sections start on the old risk?
    SELECT @offset = min(rating_section_id)
    FROM   rating_section WITH(NOLOCK)
    WHERE  risk_cnt = @original_risk_cnt
    AND    original_flag = 0

    -- So the offset is one less than this
    SELECT @offset = @offset - 1
	SET @offset = ISNULL(@offset, 0)

    SELECT @old_premium = this_premium
    FROM   rating_section WITH(NOLOCK)
    WHERE  risk_cnt = @original_risk_cnt
    AND    rating_section_id = @rating_section_id + @offset

    SELECT @new_premium = @this_premium

    SELECT @factor = CASE
        WHEN ISNULL(@old_premium, 0) = 0 THEN 1
        ELSE @new_premium / @old_premium
        END

    INSERT INTO Peril (
        risk_cnt,
        rating_section_id,
        peril_id,
        peril_type_id,
        class_of_business_id,
        sequence_number,
        description,
        sum_insured,
        rating_sum_insured,
        rate_type_id,
        annual_rate,
        annual_premium,
        this_premium,
        coinsured_this_premium,
        coinsured_sum_insured,
        coinsured_commission,
        retained_this_premium,
        retained_sum_insured,
        lead_commission_band,
        sub_commission_band,
        lead_commission_value,
        sub_commission_value,
        tax_group,
        tax_value,
        ri_band,
        xl_band,
        is_premium,
        is_sum_insured,
        is_levy_tax     -- Thinh Nguyen 12/08/2002
            )
    SELECT    @risk_id,
        @rating_section_id, -- I think that this was the error, missed off the @
        peril_id,
        peril_type_id,
        class_of_business_id,
        sequence_number,
        description,
        -sum_insured,    -- sum_insured
        -rating_sum_insured,    -- rating_sum_insured
        isnull(rate_type_id, 1), -- some of these are null from the data transfer - make them values
        annual_rate,
        -annual_premium,    -- annual_premium,
        CASE										--Rounding on Rating Section - Populate old rounding values (@this premium)
			WHEN @factor=1 THEN @this_premium		--Do not prorate the rounding amount - Amit	
			ELSE ISNULL(this_premium * @factor,0)
		END,
        null,    -- coinsured_this_premium,
        null,    -- coinsured_sum_insured,
        null,    -- coinsured_commission,
        null,    -- retained_this_premium,
        null,    -- retained_sum_insured,
        lead_commission_band,
        sub_commission_band,
        null,    -- lead_commission_value,
        null,    -- sub_commission_value,
        tax_group,
        null,     -- tax_value,
        ri_band,
        xl_band,
        is_premium,
        is_sum_insured,
        p.is_levy_tax       -- Thinh Nguyen 12/08/2002
    FROM  peril p
    WHERE p.risk_cnt = @original_risk_cnt
    AND   p.rating_section_id = @rating_section_id + @offset

	--IF Nothing Inserted then its definately something that is extra
	
	SELECT @rowcount=@@ROWCOUNT
	IF @rowcount=0
	BEGIN
	    INSERT INTO Peril (
	        risk_cnt,
	        rating_section_id,
	        peril_id,
	        peril_type_id,
	        class_of_business_id,
	        sequence_number,
	        description,
	        sum_insured,
	        rating_sum_insured,
	        rate_type_id,
	        annual_rate,
	        annual_premium,
	        this_premium,
	        lead_commission_band,
	        sub_commission_band,
	        tax_group,
	        ri_band,
	        xl_band,
	        is_premium,
	        is_sum_insured,
	        is_levy_tax    
	            )
	    SELECT    @risk_id,
	        @rating_section_id, 
	        p.peril_id,
	        p.peril_type_id,
	        p.class_of_business_id,
	        p.sequence_number,
	        p.description,
	        -p.sum_insured,
	        -p.rating_sum_insured,
	        isnull(p.rate_type_id, 1),
	        p.annual_rate,
	        -p.annual_premium,
	        CASE							
			WHEN @factor=1 THEN @this_premium		
			ELSE ISNULL(rs.this_premium * @factor,0)
		END,
	        lead_commission_band,
	        sub_commission_band,
	        tax_group,
	        ri_band,
	        xl_band,
	        is_premium,
	        is_sum_insured,
	        p.is_levy_tax  
	    FROM  peril p 
		JOIN rating_section rs 
		ON p.risk_cnt=rs.risk_cnt
		AND rs.rating_section_id=p.rating_section_id		
	    WHERE p.risk_cnt = @original_risk_cnt
		AND rs.rating_section_id=@rating_section_id		
	END
	
END
ELSE
--*/
BEGIN
    /* Get rate_type & peril_group_id */
    /* Rate type is added as parameter. So, get only Peril group */
    SELECT  @peril_group_id = peril_group_id
    FROM    Rating_Section_Type WITH(NOLOCK)
    WHERE   rating_section_type_id = @rating_section_type_id

    /* Exit if no peril groups */
    IF @peril_group_id is NULL
        return

    /* Check if one peril only */
    SELECT  @peril_count = COUNT(U.peril_type_id)
    FROM    Peril_Type_Usage    U WITH(NOLOCK)
    WHERE   U.peril_group_id = @peril_group_id

    /* Declare peril_groups cursor */
    DECLARE c_peril_groups CURSOR FAST_FORWARD FOR
    SELECT  U.peril_type_id,
        U.allocate_percent,
        PT.class_of_business_id,
        PT.is_sum_insured,
        PT.lead_commission_band,
        PT.sub_commission_band,
        PT.tax_group,
        PT.is_premium,
        PT.ri_band ,
        PT.xl_band,
        PT.is_levy_tax 
    FROM    Peril_Type_Usage    U WITH(NOLOCK),
        Peril_Type      PT WITH(NOLOCK),
        Peril_Group     G WITH(NOLOCK),
        Rating_Section_Type RT WITH(NOLOCK),
        Rating_Section      S WITH(NOLOCK)
    WHERE   U.peril_group_id = G.peril_group_id
    AND G.peril_group_id = RT.peril_group_id
    AND RT.rating_section_type_id = S.rating_section_type_id
    AND S.risk_cnt = @risk_id
    AND S.rating_section_id = @rating_section_id
    AND PT.peril_type_id = U.peril_type_id

    /* Open the cursor */
    OPEN c_peril_groups

    /* Fetch 1st row */
    FETCH NEXT FROM c_peril_groups
    INTO    @peril_type_id,
        @allocate_percent,
        @class_of_business_id,
        @is_sum_insured,
        @lead_commission_band,
        @sub_commission_band,
        @tax_group,
        @is_premium,
        @ri_band,
        @xl_band,
        @is_levy_tax        -- Thinh Nguyen 12/08/2002

    /* Loop through cursor */
    WHILE @@FETCH_STATUS <> -1
    BEGIN
        /* Get next peril_id */
        SELECT  @peril_id = MAX(peril_id) + 1
        FROM    Peril
        WHERE   risk_cnt = @risk_id
        AND rating_section_id = @rating_section_id

        IF ISNULL(@peril_id, 0) = 0
            SELECT  @peril_id = 1

        /* Calculate annual_rate */
        SELECT @final_annual_rate = @annual_rate
        IF @peril_count > 1
            EXEC    spu_pm_calc_percent_value      
    @total_value = @annual_rate,    
                                @percentage = @allocate_percent,
                                @number_of_dp = @insurance_file_no_of_dp,
                                @o_percentage_value = @final_annual_rate OUTPUT

        /* Calculate annual_premium */
        SELECT @final_annual_premium = @annual_premium
        IF @peril_count > 1
            EXEC    spu_pm_calc_percent_value    @total_value = @annual_premium,
                                @percentage = @allocate_percent,
                                @number_of_dp = @insurance_file_no_of_dp,
                                @o_percentage_value = @final_annual_premium OUTPUT

        /* Calculate this_premium */
        SELECT @final_this_premium = @this_premium
        IF @peril_count > 1
            EXEC    spu_pm_calc_percent_value    @total_value = @this_premium,
                                @percentage = @allocate_percent,
                                @number_of_dp = @insurance_file_no_of_dp,
                                @o_percentage_value = @final_this_premium OUTPUT

        /* If this is a sum insured carrying peril */
        IF (@is_sum_insured = 1)
                        /* sum insured equals the sum insured */
            SELECT @peril_sum_insured = @sum_insured
        ELSE
                        /* sum insured equals zero */
            SELECT @peril_sum_insured = 0

        /* Add Peril */
        INSERT INTO Peril (
            risk_cnt,
            rating_section_id,
            peril_id,
            peril_type_id,
            class_of_business_id,
            sequence_number,
            description,
            sum_insured,
            rating_sum_insured,
            rate_type_id,
            annual_rate,
            annual_premium,
            this_premium,
            coinsured_this_premium,
            coinsured_sum_insured,
            coinsured_commission,
            retained_this_premium,
            retained_sum_insured,
            lead_commission_band,
            sub_commission_band,
            lead_commission_value,
            sub_commission_value,
            tax_group,
            tax_value,
            ri_band,
            xl_band,
            is_premium,
            is_sum_insured,
            is_levy_tax)    -- Thinh Nguyen 12/08/2002

        VALUES  (
            @risk_id,
            @rating_section_id,
            @peril_id,
            @peril_type_id,
            @class_of_business_id,
            @peril_id,
            NULL,
            @peril_sum_insured,
            @sum_insured,
            @rate_type_id,
            @final_annual_rate,
            @final_annual_premium,
            ISNULL(@final_this_premium,0),
            NULL,
            NULL,
            NULL,
            NULL,
            NULL,
            @lead_commission_band,
            @sub_commission_band,
            NULL,
            NULL,
            @tax_group,
            NULL,
            @ri_band,
            @xl_band,
            @is_premium,
            @is_sum_insured,
            @is_levy_tax)       -- Thinh Nguyen 12/08/2002

        /* Fetch next row */
        FETCH NEXT FROM c_peril_groups
        INTO    @peril_type_id,
            @allocate_percent,
            @class_of_business_id,
            @is_sum_insured,
            @lead_commission_band,
            @sub_commission_band,
            @tax_group,
            @is_premium,
            @ri_band,
            @xl_band,
            @is_levy_tax        -- Thinh Nguyen 12/08/2002

    END

    /* Destroy the cursor */
    CLOSE       c_peril_groups
    DEALLOCATE  c_peril_groups

    /* Check Peril values */
    IF @peril_count > 1
    BEGIN
        /* Adjust annual_rate */
        SELECT  @total_perils = SUM(annual_rate)
        FROM    Peril WITH(NOLOCK)
        WHERE   risk_cnt = @risk_id
        AND rating_section_id = @rating_section_id

        IF @total_perils <> @annual_rate
        BEGIN
            UPDATE  Peril
            SET annual_rate = annual_rate + (@annual_rate - @total_perils)
            WHERE   risk_cnt = @risk_id
            AND rating_section_id = @rating_section_id
            AND peril_id = 1
        END

        /* Adjust annual_premium */
        SELECT  @total_perils = SUM(annual_premium)
        FROM    Peril WITH(NOLOCK)
        WHERE   risk_cnt = @risk_id
        AND rating_section_id = @rating_section_id

        IF @total_perils <> @annual_premium
        BEGIN
            UPDATE  Peril
            SET annual_premium = annual_premium + (@annual_premium - @total_perils)
            WHERE   risk_cnt = @risk_id
            AND rating_section_id = @rating_section_id
            AND peril_id = 1
        END

        /* Adjust this_premium */
        SELECT  @total_perils = SUM(this_premium)
        FROM    Peril WITH(NOLOCK)
        WHERE   risk_cnt = @risk_id
        AND rating_section_id = @rating_section_id

        IF @total_perils <> @this_premium
        BEGIN
            UPDATE  Peril
            SET this_premium = this_premium + (@this_premium - @total_perils)
            WHERE   risk_cnt = @risk_id
            AND rating_section_id = @rating_section_id
            AND peril_id = 1
        END
    END
END

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
