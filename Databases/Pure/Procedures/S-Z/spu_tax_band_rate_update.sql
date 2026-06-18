SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

Execute DDLDropProcedure 'spu_tax_band_rate_update'
GO

CREATE PROCEDURE spu_tax_band_rate_update
    @tax_band_id int,
    @tax_band_rate_id int,
    @code char(10),
    @caption_id int,
    @description varchar(255),
    @effective_date datetime,
    @is_deleted tinyint,
    @is_value tinyint,
    @rate float, 
    @Calc_basis int, 
    @Basis_value money, 
    @NB tinyint,
    @AMTA tinyint, 
    @RMTA tinyint, 
    @CANC tinyint, 
    @REN tinyint,
    @sum_insured_rounded tinyint,
    @currency_id smallint,
    @allow_tax_credit tinyint,
    @country_id int,
    @state_id int,
    @class_of_business_id int,
    @TTRI tinyint,
    @TTRIC tinyint,
    @TTAC tinyint,
    @TTF tinyint,
    @TTCP tinyint,
    @TTCS tinyint,
    @TTCR tinyint,
    @TTIC tinyint,
    @TTI tinyint,
    @MTA_threshold_date datetime,
    @Is_passed_to_insurer tinyint,
    @TTE tinyint,
    @risk_group_id int,
    @risk_code_id int,
    @COB_rating_section_id int,	
	@is_suspended tinyint, 
	@suspended_caption Char(1), 
	@TTRIPR tinyint
AS
    
    UPDATE tax_band_rate SET
        tax_band_id=@tax_band_id,
        --tax_band_rate_id=@tax_band_rate_id,
        code=@code,
        caption_id=@caption_id,
        description=@description,
        effective_date=@effective_date,
        is_deleted=@is_deleted,
        is_value=@is_value,
        rate=@rate, 
        calc_basis=@calc_basis, 
        Basis_value=@Basis_value, 
        NB=@NB, 
        AMTA=@AMTA, 
        RMTA=@RMTA, 
        CANC=@CANC, 
        REN=@REN,
        sum_insured_rounded=@sum_insured_rounded,
        currency_id=@currency_id,
        allow_tax_credit=@allow_tax_credit,
        country_id=@country_id,
        state_id=@state_id,
        class_of_business_id=@class_of_business_id,
        TTRI=@TTRI,
        TTRIC=@TTRIC,
        TTAC=@TTAC,
        TTF=@TTF,
        TTCP=@TTCP,
        TTCS=@TTCS,
        TTCR=@TTCR,
		TTIC=@TTIC,
		TTI=@TTI,
		MTA_Threshold_date=@MTA_Threshold_date,
		Is_passed_to_insurer=@Is_passed_to_insurer,
		TTE=@TTE,
    	risk_group_id=@risk_group_id,
    	risk_code_id=@risk_code_id,
		COB_rating_section_id=@COB_rating_section_id,
		-- E001
		is_suspended=@is_suspended, 
		suspended_account_code_suffix=@suspended_caption, 
		TTRIPR=@TTRIPR
	WHERE tax_band_id=@tax_band_id AND tax_band_rate_id=@tax_band_rate_id

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO