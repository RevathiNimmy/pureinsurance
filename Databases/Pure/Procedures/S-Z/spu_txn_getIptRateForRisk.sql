ddldropprocedure spu_txn_getIptRateForRisk
go

CREATE PROCEDURE spu_txn_getIptRateForRisk
(
@RiskCodeId int,
@InsurerCnt int
)

AS

DECLARE @DomiciledForTax tinyint
DECLARE @COBRatingSectionId int
DECLARE @RiskGroupId int
DECLARE @Tax_Group_Id int
DECLARE @CountryId int

SELECT @CountryId = isnull(country_id,0) from Country where code = 'GBR'

SELECT @COBRatingSectionId = isnull(COB_Rating_Section_id,0) FROM Cob_rating_section where code = 'IPTPREMIUM'

select @Tax_Group_id = isnull(tax_group_id,0) from tax_group where code = 'IPT'

select @RiskGroupId = risk_group_id from risk_code where risk_code_id = @riskcodeid

if @InsurerCnt <> 0
Begin
	SELECT @DomiciledForTax=ISNULL(domiciled_for_tax,0) FROM party_insurer WHERE party_cnt=@InsurerCnt

	IF @DomiciledForTax=0
	BEGIN
		RETURN
	END
End
Else
Begin
	Set @DomiciledForTax = 1
End

CREATE TABLE #TXN_SECTION_RATE_SELECT
(
tax_group_id int NULL,
tax_group_code varchar(10) NULL,
rate float NULL,
risk_group_id int NULL,
risk_code_id int NULL,
COB_rating_section_id int NULL,
effective_date datetime
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
	TBR.NB = 1 AND TG.TAX_GROUP_ID=@Tax_Group_Id
)
BEGIN
	INSERT INTO #TXN_SECTION_RATE_SELECT
	SELECT	TG.tax_group_id,
	TG.code,
	TBR.rate,
	TBR.risk_group_id,
	TBR.risk_code_id,
	TBR.COB_rating_section_id,
	TBR.effective_date
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
	((@DomiciledForTax=1 AND ISNULL(TG.is_withholding_tax,0)=0) OR
	(@DomiciledForTax=0 AND ISNULL(TG.is_withholding_tax,0)=1))  AND
	TBR.NB = 1 AND TG.TAX_GROUP_ID=@Tax_Group_Id
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
	TBR.effective_date
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
	TBR.NB = 1 AND TG.TAX_GROUP_ID=@Tax_Group_Id
END

IF EXISTS(SELECT NULL FROM #TXN_SECTION_RATE_SELECT WHERE COB_rating_section_id=@COBRatingSectionId)
BEGIN
	SELECT effective_date, rate
	FROM
	#TXN_SECTION_RATE_SELECT
	WHERE
	COB_rating_section_id=@COBRatingSectionId
END
ELSE
BEGIN

	IF EXISTS(SELECT NULL FROM #TXN_SECTION_RATE_SELECT WHERE risk_code_id=@RiskCodeId)
	BEGIN

		SELECT effective_date, rate
		FROM
		#TXN_SECTION_RATE_SELECT
		WHERE
		risk_code_id=@RiskCodeId

	END
	ELSE
	BEGIN

		IF EXISTS(SELECT NULL FROM #TXN_SECTION_RATE_SELECT WHERE risk_group_id=@RiskGroupId)
		BEGIN

			SELECT effective_date, rate
			FROM
			#TXN_SECTION_RATE_SELECT
			WHERE
			risk_group_id=@RiskGroupId

		END
		ELSE
		BEGIN

			SELECT effective_date, rate
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
		END
	END
END

DROP TABLE #TXN_SECTION_RATE_SELECT
