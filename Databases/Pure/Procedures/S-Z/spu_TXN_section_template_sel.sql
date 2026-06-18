SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_TXN_section_template_sel'
GO

CREATE PROCEDURE spu_TXN_section_template_sel
(

	@RiskCodeId int,
	@RiskGroupId int,
	@CountryId int,
	@TransactionType int,
	@EffectiveDate datetime,
	@InsurerId int
)
AS

DECLARE @iCOBRatingSectionId int
DECLARE @iTaxGroupId int
DECLARE @vTaxGroupCode varchar(10)
DECLARE @fTaxRate float
DECLARE @icalcBasis int
DECLARE @iTaxEditable tinyint

CREATE TABLE
#TXN_SECTION_TEMPLATE_SEL
(
	COB_rating_section_id int,
	COB_rating_section_description varchar(255),
	is_levy_section tinyint,
	tax_group_id int NULL,
	tax_group_code varchar(10) NULL,
	tax_percent float NULL,
	is_in_TP_commission_calculation tinyint NULL,
	is_in_TP_premium_calculation tinyint NULL,
	calc_basis int NULL,
	tax_editable int NULL
)

INSERT INTO #TXN_SECTION_TEMPLATE_SEL
SELECT
COB.COB_rating_section_id,
COB.description,
COB.is_levy_section,
NULL,
NULL,
NULL,
COB.is_in_TP_commission_calculation,
COB.is_in_TP_premium_calculation,
NULL
FROM
Risk_Tax_Usage RTU
INNER JOIN COB_rating_section COB ON COB.COB_rating_section_id=RTU.COB_rating_section_id
WHERE RTU.risk_code_id=@RiskCodeId
ORDER BY RTU.sequence ASC

DECLARE TXN_SEC_TEMPLATE_CURSOR CURSOR FOR
SELECT
COB_rating_section_id
FROM
#TXN_SECTION_TEMPLATE_SEL

OPEN TXN_SEC_TEMPLATE_CURSOR

FETCH NEXT FROM TXN_SEC_TEMPLATE_CURSOR
INTO @iCOBRatingSectionId

WHILE @@FETCH_STATUS = 0
BEGIN
	EXECUTE spu_TXN_section_tax_rate_sel
	NULL,
	0,
	@RiskGroupId,
	@RiskCodeId,
	@iCOBRatingSectionId,
	@CountryId,
	@TransactionType,
	@EffectiveDate,
	@InsurerId,
	@rTaxGroupId= @iTaxGroupId OUTPUT,
	@rTaxGroupCode= @vTaxGroupCode OUTPUT,
   	@rTaxRate= @fTaxRate OUTPUT,
	@rcalcBasis=@icalcBasis OUTPUT,
	@rTaxEditable=@iTaxEditable OUTPUT

   	UPDATE #TXN_SECTION_TEMPLATE_SEL
   	SET
   	tax_group_id = @iTaxGroupId,
	tax_group_code = @vTaxGroupCode,
	tax_percent = @fTaxRate,
	calc_basis=@icalcBasis,
	tax_editable=@iTaxEditable
	where
	COB_rating_section_id=@iCOBRatingSectionId

   	FETCH NEXT FROM TXN_SEC_TEMPLATE_CURSOR
   	INTO @iCOBRatingSectionId
END


CLOSE TXN_SEC_TEMPLATE_CURSOR
DEALLOCATE TXN_SEC_TEMPLATE_CURSOR

SELECT * FROM #TXN_SECTION_TEMPLATE_SEL

DROP TABLE #TXN_SECTION_TEMPLATE_SEL

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
