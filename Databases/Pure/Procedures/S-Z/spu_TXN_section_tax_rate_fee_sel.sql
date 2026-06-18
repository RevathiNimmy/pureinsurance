ddldropprocedure spu_TXN_section_tax_rate_fee_sel
go

CREATE PROCEDURE spu_TXN_section_tax_rate_fee_sel
(
@TaxGroupId int,
@RiskGroupId int,
@RiskCodeId int,
@CountryId int,
@EffectiveDate datetime,
@PartyCnt int,
@rTaxRate float OUTPUT
)

AS


declare @PartyType varchar(10)

select @PartyType = party_type.code from party join party_type on party_type.party_type_id = party.party_type_id where party_cnt=@PartyCnt

CREATE TABLE #TXN_SECTION_RATE_FEE_SEL
(
tax_group_id int NULL,
tax_group_code varchar(10) NULL,
rate float NULL,
NB tinyint NULL,
AMTA tinyint NULL,
RMTA tinyint NULL,
CANC tinyint NULL,
REN tinyint NULL,
TTAC tinyint NULL,
TTF tinyint NULL,
TTIC tinyint NULL,
TTE tinyint NULL,
risk_group_id int NULL,
risk_code_id int NULL,
COB_rating_section_id int NULL,
calc_basis int Null
)

IF EXISTS
( 	SELECT NULL FROM Tax_Group TG
	INNER JOIN Tax_Group_Tax_Band TGTB ON TG.tax_group_id=TGTB.tax_group_id
	INNER JOIN Tax_Band TB ON TGTB.tax_band_id=TB.tax_band_id
	INNER JOIN Tax_Band_Rate TBR ON TB.tax_band_id=TBR.tax_band_id
	WHERE
	TG.tax_group_id=@TaxGroupId AND
	TG.is_deleted=0 AND
	TG.effective_date <= @EffectiveDate AND
	TB.is_deleted=0 AND
	TB.effective_date <= @EffectiveDate AND
	TBR.is_deleted=0 AND
	TBR.effective_date <= @EffectiveDate AND
	TBR.country_id = @CountryId AND
	((TBR.TTF = 1 AND @PartyType='FE') OR
	(TBR.TTE = 1 AND @PartyType='EX'))

)
BEGIN
	INSERT INTO #TXN_SECTION_RATE_FEE_SEL
	SELECT	TG.tax_group_id,
	TG.code,
	TBR.rate,
	TBR.NB,
	TBR.AMTA,
	TBR.RMTA,
	TBR.CANC,
	TBR.REN,
	TBR.TTAC,
	TBR.TTF,
	TBR.TTIC,
	TBR.TTE,
	TBR.risk_group_id,
	TBR.risk_code_id,
	TBR.COB_rating_section_id,
	TBR.calc_basis
	FROM
	Tax_Group TG
	INNER JOIN Tax_Group_Tax_Band TGTB ON TG.tax_group_id=TGTB.tax_group_id
	INNER JOIN Tax_Band TB ON TGTB.tax_band_id=TB.tax_band_id
	INNER JOIN Tax_Band_Rate TBR ON TB.tax_band_id=TBR.tax_band_id
	WHERE
	TG.tax_group_id=@TaxGroupId AND
	TG.is_deleted=0 AND
	TG.effective_date <= @EffectiveDate AND
	TB.is_deleted=0 AND
	TB.effective_date <= @EffectiveDate AND
	TBR.is_deleted=0 AND
	TBR.effective_date <= @EffectiveDate AND
	TBR.country_id = @CountryId AND
	((TBR.TTF = 1 AND @PartyType='FE') OR
	(TBR.TTE = 1 AND @PartyType='EX'))
END
ELSE
BEGIN
	INSERT INTO #TXN_SECTION_RATE_FEE_SEL
	SELECT	TG.tax_group_id,
	TG.code,
	TBR.rate,
	TBR.NB,
	TBR.AMTA,
	TBR.RMTA,
	TBR.CANC,
	TBR.REN,
	TBR.TTAC,
	TBR.TTF,
	TBR.TTIC,
	TBR.TTE,
	TBR.risk_group_id,
	TBR.risk_code_id,
	TBR.COB_rating_section_id,
	TBR.calc_basis
	FROM
	Tax_Group TG
	INNER JOIN Tax_Group_Tax_Band TGTB ON TG.tax_group_id=TGTB.tax_group_id
	INNER JOIN Tax_Band TB ON TGTB.tax_band_id=TB.tax_band_id
	INNER JOIN Tax_Band_Rate TBR ON TB.tax_band_id=TBR.tax_band_id
	WHERE
	TG.tax_group_id=@TaxGroupId AND
	TG.is_deleted=0 AND
	TG.effective_date <= @EffectiveDate AND
	TB.is_deleted=0 AND
	TB.effective_date <= @EffectiveDate AND
	TBR.is_deleted=0 AND
	TBR.effective_date <= @EffectiveDate AND
	TBR.country_id IS NULL AND
	((TBR.TTF = 1 AND @PartyType='FE') OR
	(TBR.TTE = 1 AND @PartyType='EX'))
END


IF EXISTS(SELECT NULL FROM #TXN_SECTION_RATE_FEE_SEL WHERE COB_rating_section_id IS NOT NULL)  
BEGIN  
 	SELECT TOP 1 @rTaxRate = rate 
 	FROM #TXN_SECTION_RATE_FEE_SEL  
 	WHERE COB_rating_section_id IS NOT NULL  
END 
ELSE
BEGIN
	IF EXISTS(SELECT NULL FROM #TXN_SECTION_RATE_FEE_SEL WHERE risk_code_id=@RiskCodeId)
	BEGIN

		SELECT TOP 1
		@rTaxRate=rate
		FROM
		#TXN_SECTION_RATE_FEE_SEL
		WHERE
		risk_code_id=@RiskCodeId

	END
	ELSE
	BEGIN

		IF EXISTS(SELECT NULL FROM #TXN_SECTION_RATE_FEE_SEL WHERE risk_group_id=@RiskGroupId)
		BEGIN

			SELECT TOP 1
			@rTaxRate=rate
			FROM
			#TXN_SECTION_RATE_FEE_SEL
			WHERE
			risk_group_id=@RiskGroupId

		END
		ELSE
		BEGIN

			SELECT TOP 1
			@rTaxRate=rate
			FROM
			#TXN_SECTION_RATE_FEE_SEL
			WHERE
			(risk_group_id IS NULL) AND
			(risk_code_id IS NULL) AND
			(COB_rating_section_id IS NULL)
			OR
			(risk_group_id = 0 AND
			risk_code_id = 0 AND
			COB_rating_section_id = 0)
		END
	END
END
if  (@rTaxRate) is null 
Begin 
	set @rTaxRate = 0 
End

DROP TABLE #TXN_SECTION_RATE_FEE_SEL
