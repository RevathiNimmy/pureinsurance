SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_TXN_insurer_section_template_sel'
GO

CREATE PROCEDURE spu_TXN_insurer_section_template_sel

@PartyCnt int,
@RiskCodeId int,
@RiskGroupId int,
--@COBRatingSectionId int,
@CountryId int,
@TransactionType int,
@EffectiveDate datetime,
@GisSchemeId int = 0

AS

CREATE TABLE #TXN_INSURER_SECTION
(
COB_rating_section_id int,
COB_rating_section_description varchar(255),
is_levy_section tinyint,
tax_group_id int NULL,
tax_group_code varchar(10) NULL,
tax_percent float NULL,
commission_percent numeric(7,4) NULL,
commission_amount numeric(19,4) NULL,
minimum_brokerage numeric(19,4) NULL,
calc_basis int NULL
)

INSERT INTO #TXN_INSURER_SECTION
SELECT	COB.COB_rating_section_id,
	COB.description,
	COB.is_levy_section,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL
FROM
	Risk_Tax_Usage RTU
	INNER JOIN COB_rating_section COB ON COB.COB_rating_section_id=RTU.COB_rating_section_id
	WHERE RTU.risk_code_id=@RiskCodeId
	ORDER BY RTU.sequence ASC

DECLARE @iCOBRatingSectionId int,
	@nCommissionPercent numeric(7,4),
	@nCommissionAmount numeric(19,4),
	@nMinimumBrokerage numeric(19,4),
	@iTaxGroupId int,
	@iTGId int,
	@vTaxGroupCode varchar(10),
	@fTaxRate float,
	@icalcBasis int,
	@iTaxEditable tinyint

DECLARE CURSOR_Insurer_Section CURSOR FOR
	SELECT COB_rating_section_id
	FROM #TXN_INSURER_SECTION

OPEN CURSOR_Insurer_Section

FETCH NEXT FROM CURSOR_Insurer_Section INTO
	@iCOBRatingSectionId

WHILE @@FETCH_STATUS = 0 BEGIN

EXECUTE spu_TXN_insurer_commission_rate_sel
	@PartyCnt,
	@RiskGroupId,
	@RiskCodeId,
	@iCOBRatingSectionId,
	@TransactionType,
	@EffectiveDate,
	@rCommissionPercent=@nCommissionPercent OUTPUT,
	@rCommissionAmount=@nCommissionAmount OUTPUT,
	@rMinimumBrokerage=@nMinimumBrokerage OUTPUT,
	@rTaxGroupId=@iTaxGroupId OUTPUT,
	@GisSchemeId=@GisSchemeId

	UPDATE #TXN_INSURER_SECTION
	SET
	commission_percent=@nCommissionPercent,
	commission_amount=@nCommissionAmount,
	minimum_brokerage=@nMinimumBrokerage
	WHERE
	COB_rating_section_id=@iCOBRatingSectionId

EXECUTE spu_TXN_section_tax_rate_sel
	@iTaxGroupId,
	1,
	@RiskGroupId,
	@RiskCodeId,
	@iCOBRatingSectionId,
	@CountryId,
	@TransactionType,
	@EffectiveDate,
	@PartyCnt,
	@rTaxGroupId=@iTGId OUTPUT,
	@rTaxGroupCode=@vTaxGroupCode OUTPUT,
   	@rTaxRate=@fTaxRate OUTPUT,
	@rcalcBasis=@icalcBasis OUTPUT,
	@rTaxEditable=@iTaxEditable OUTPUT

	UPDATE #TXN_INSURER_SECTION
	SET
	tax_group_id=@iTGId,
	tax_group_code=@vTaxGroupCode,
	tax_percent=@fTaxRate,
	calc_basis=@icalcBasis
	WHERE
	COB_rating_section_id=@iCOBRatingSectionId

	FETCH NEXT FROM CURSOR_Insurer_Section
   	INTO @iCOBRatingSectionId
END
	
CLOSE CURSOR_Insurer_Section
DEALLOCATE CURSOR_Insurer_Section

SELECT * FROM #TXN_INSURER_SECTION

DROP TABLE #TXN_INSURER_SECTION

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

