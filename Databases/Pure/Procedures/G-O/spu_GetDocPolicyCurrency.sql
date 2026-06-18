SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_GetDocCurrency'
GO


CREATE PROCEDURE spu_GetDocCurrency
    @InsuranceFileCnt int,
    @DocumentRef varchar(25),
    @PartyCnt int = Null
AS

IF @InsuranceFileCnt <> 0
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
ELSE
	SELECT	TOP 1 
			c.currency_id,
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
	    FROM document d 
	    JOIN transdetail t ON t.document_id = d.document_id
	    JOIN currency c ON c.currency_id = t.currency_id
	    JOIN cashlistitem csh ON csh.transdetail_id = t.transdetail_id
	    JOIN cashlist cl ON csh.cashlist_id = cl.cashlist_id
	    JOIN cashlisttype clt ON cl.cashlisttype_id = clt.cashlisttype_id
	    JOIN mediatype mt ON csh.mediatype_id = mt.mediatype_id
	    JOIN account a ON a.account_id = csh.account_id
	    JOIN party p ON p.party_cnt = a.account_key
	    JOIN country ON csh.address_country = country.country_id
	    JOIN PMUser pm ON csh.pmuser_id=pm.user_id
	    WHERE d.document_ref = LTRIM(@documentRef )
	    AND p.party_cnt = ISNULL(STR(@PartyCnt), p.party_cnt)
	    Order By t.transdetail_id DESC
GO