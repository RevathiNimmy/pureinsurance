SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_GetPolicyCurrency'
GO


CREATE PROCEDURE spu_GetPolicyCurrency
    @InsuranceFileCnt int
AS

SELECT	c.currency_id,
		c.caption_id,
		c.iso_code,
		c.description,
		c.minor_part,
		c.code,
		c.symbol,
		c.alignment,
		c.decimal_places,
		c.is_deleted,
		c.effective_date,
		c.format_string,
		c.round_to_places
FROM	Insurance_File ifi Join Currency c ON ifi.currency_id = c.currency_id
WHERE	ifi.insurance_file_cnt = @InsuranceFileCnt

GO