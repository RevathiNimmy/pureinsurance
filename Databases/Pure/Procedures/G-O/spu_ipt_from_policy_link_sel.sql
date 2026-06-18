SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON
GO


EXECUTE DDLDropProcedure 'spu_ipt_from_policy_link_sel'
GO


CREATE PROCEDURE spu_ipt_from_policy_link_sel
    @gis_policy_link_id INTEGER,
    @effective_date DATETIME
AS

BEGIN
	DECLARE @AgentUnderwriter varchar(1) 
	
	SELECT  @AgentUnderwriter = value  
	FROM    hidden_options  
	WHERE   branch_id = 1 and option_number = 1 
	
	if @AgentUnderwriter = 'A' 
	Begin
		declare @iCOBRatingSectionId integer
		declare @RiskGroupId integer
		declare @RiskCodeId integer
		declare @CountryId integer
		declare @InsurerId integer
		declare @EffectiveDate datetime
		declare @iTaxGroupId integer
		declare @vTaxGroupCode varchar(10)
		declare @fTaxRate float
		declare @iCalcBasis integer
		declare @iTaxEditable integer
		
		select @EffectiveDate=getdate()
		
		select 
			@CountryId=insurance_file.country_id, 
			@RiskCodeId=insurance_file.risk_code_id, 
			@InsurerId=insurance_file.lead_insurer_cnt, 
			@RiskGroupId=risk_code.risk_group_id 
		from 
			gis_policy_link
			join insurance_file on insurance_file.insurance_file_cnt = gis_policy_link.insurance_file_cnt
			join risk_code on risk_code.risk_code_id = insurance_file.risk_code_id
		where gis_policy_link.gis_policy_link_id=1309
		
		select @iCOBRatingSectionId = cob_rating_section_id from cob_rating_section where code = 'IPTPREMIUM'
		
		EXECUTE spu_TXN_section_tax_rate_sel
			NULL,
			0,
			@RiskGroupId,
			@RiskCodeId,
			@iCOBRatingSectionId,
			@CountryId,
			0,
			@EffectiveDate,
			@InsurerId,
			@rTaxGroupId= @iTaxGroupId OUTPUT,
			@rTaxGroupCode= @vTaxGroupCode OUTPUT,
		   	@rTaxRate= @fTaxRate OUTPUT,
			@rcalcBasis=@icalcBasis OUTPUT,
			@rTaxEditable=@iTaxEditable OUTPUT

		SELECT @fTaxRate
	End
	Else
	Begin
		SELECT t.rate
		FROM ipt t,
		     insurance_file i,
		     gis_policy_link l
		WHERE t.risk_code_id = i.risk_code_id
		AND  l.insurance_file_cnt = i.insurance_file_cnt
		AND  l.gis_policy_link_id = @gis_policy_link_id
		AND @effective_date >= t.effective_date
		ORDER BY t.effective_date desc
	End
END
GO


SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON
GO


