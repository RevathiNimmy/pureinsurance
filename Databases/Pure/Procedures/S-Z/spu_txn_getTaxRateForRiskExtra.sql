ddldropprocedure spu_txn_getTaxRateForRiskExtra
go

CREATE PROCEDURE spu_txn_getTaxRateForRiskExtra
(
@RiskCodeId int,
@PartyCnt int,
@taxgroupid int = null output
)

AS

DECLARE @RiskGroupId int
DECLARE @CountryId int
Declare @PartyTypeCode varchar(25)
Declare @ExtraTaxGroupId int

SELECT @CountryId = isnull(country_id,0) from Country where code = 'GBR'

select @RiskGroupId = risk_group_id from risk_code where risk_code_id = @riskcodeid

select @ExtraTaxGroupId = isnull(tax_group_id,0) from fee_amounts where party_cnt=@PartyCnt and Risk_Group_Id=@riskGroupId

select @PartyTypeCode = party_type.code from party_type
join party on party_type.party_type_id = party.party_type_id
where party_cnt=@PartyCnt

CREATE TABLE #TXN_SECTION_RATE_SELECT
(
tax_group_id int NULL,
tax_group_code varchar(10) NULL,
rate float NULL,
risk_group_id int NULL,
risk_code_id int NULL,
COB_rating_section_id int NULL,
effective_date datetime,
amount numeric(19,4),
TaxGroupCode varchar(10)
)

IF EXISTS
(
	SELECT NULL
	FROM
	Tax_Group TG
	INNER JOIN Tax_Group_Tax_Band TGTB ON TG.tax_group_id=TGTB.tax_group_id
	INNER JOIN Tax_Band TB ON TGTB.tax_band_id=TB.tax_band_id
	INNER JOIN Tax_Band_Rate TBR ON TB.tax_band_id=TBR.tax_band_id
	WHERE
	TG.is_deleted=0 AND
	TB.is_deleted=0 AND
	TBR.is_deleted=0 AND
	TBR.country_id = @CountryId AND
	((TBR.TTE = 1 and rTrim(@PartyTypeCode) = 'EX') or (TBR.TTF = 1 and rTrim(@PartyTypeCode) = 'FE')) AND
	((@ExtraTaxGroupId = 0) or (@ExtraTaxGroupId <> 0 and TG.Tax_Group_Id = @ExtraTaxGroupId))
)
BEGIN
	INSERT INTO #TXN_SECTION_RATE_SELECT
	SELECT	TG.tax_group_id,
	TG.code,
	TBR.rate,
	TBR.risk_group_id,
	TBR.risk_code_id,
	TBR.COB_rating_section_id,
	TBR.effective_date,
	0,
	TG.Code
	FROM
	Tax_Group TG
	INNER JOIN Tax_Group_Tax_Band TGTB ON TG.tax_group_id=TGTB.tax_group_id
	INNER JOIN Tax_Band TB ON TGTB.tax_band_id=TB.tax_band_id
	INNER JOIN Tax_Band_Rate TBR ON TB.tax_band_id=TBR.tax_band_id
	WHERE
	TG.is_deleted=0 AND
	TB.is_deleted=0 AND
	TBR.is_deleted=0 AND
	TBR.country_id = @CountryId AND
	((TBR.TTE = 1 and rTrim(@PartyTypeCode) = 'EX') or (TBR.TTF = 1 and rTrim(@PartyTypeCode) = 'FE')) AND
	((@ExtraTaxGroupId = 0) or (@ExtraTaxGroupId <> 0 and TG.Tax_Group_Id = @ExtraTaxGroupId))
END
ELSE
BEGIN
	INSERT INTO #TXN_SECTION_RATE_SELECT
	SELECT	TG.tax_group_id,
	TG.code,
	TBR.rate,
	TBR.risk_group_id,
	TBR.risk_code_id,
	TBR.COB_rating_section_id,
	TBR.effective_date,
	0,
	TG.Code
	FROM
	Tax_Group TG
	INNER JOIN Tax_Group_Tax_Band TGTB ON TG.tax_group_id=TGTB.tax_group_id
	INNER JOIN Tax_Band TB ON TGTB.tax_band_id=TB.tax_band_id
	INNER JOIN Tax_Band_Rate TBR ON TB.tax_band_id=TBR.tax_band_id
	WHERE
	TG.is_deleted=0 AND
	TB.is_deleted=0 AND
	TBR.is_deleted=0 AND
	TBR.country_id IS NULL AND
	((TBR.TTE = 1 and rTrim(@PartyTypeCode) = 'EX') or (TBR.TTF = 1 and rTrim(@PartyTypeCode) = 'FE')) AND
	((@ExtraTaxGroupId = 0) or (@ExtraTaxGroupId <> 0 and TG.Tax_Group_Id = @ExtraTaxGroupId))
END

IF EXISTS(SELECT NULL FROM #TXN_SECTION_RATE_SELECT WHERE risk_code_id=@RiskCodeId)
BEGIN

	SELECT effective_date, rate, amount, TaxGroupCode
	FROM
	#TXN_SECTION_RATE_SELECT
	WHERE
	risk_code_id=@RiskCodeId

	SELECT @taxgroupid = (select top 1 tax_group_id
	FROM
	#TXN_SECTION_RATE_SELECT
	WHERE
	risk_code_id=@RiskCodeId)

END
ELSE
BEGIN

	IF EXISTS(SELECT NULL FROM #TXN_SECTION_RATE_SELECT WHERE risk_group_id=@RiskGroupId)
	BEGIN

		SELECT effective_date, rate, amount, TaxGroupCode
		FROM
		#TXN_SECTION_RATE_SELECT
		WHERE
		risk_group_id=@RiskGroupId

		SELECT @taxgroupid = (select top 1 tax_group_id
		FROM
		#TXN_SECTION_RATE_SELECT
		WHERE
		risk_group_id=@RiskgroupId)

	END
	ELSE
	BEGIN
		SELECT effective_date, rate, amount, TaxGroupCode
		FROM
		#TXN_SECTION_RATE_SELECT
		WHERE
		(risk_group_id IS NULL) AND
		(risk_code_id IS NULL) AND
		(COB_rating_section_id IS NULL)
		OR
		(risk_group_id = 0 AND
		risk_code_id = 0 AND
		COB_rating_section_id = 0)

		SELECT @taxgroupid = (select top 1 tax_group_id
		FROM
		#TXN_SECTION_RATE_SELECT
		WHERE
		(risk_group_id IS NULL) AND
		(risk_code_id IS NULL) AND
		(COB_rating_section_id IS NULL)
		OR
		(risk_group_id = 0 AND
		risk_code_id = 0 AND
		COB_rating_section_id = 0))
	END
END

DROP TABLE #TXN_SECTION_RATE_SELECT
