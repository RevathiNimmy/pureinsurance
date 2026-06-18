SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Fee_amounts_sel_by_risk'
GO

-- AMB 03/10/2003: Accident Management RFC changes

CREATE PROCEDURE spu_Fee_amounts_sel_by_risk
    @language_id  int,
    @code         varchar(25)
AS

SELECT  
    p.shortname, 
    fa.risk_group_id, 
    pc.caption, 
    fa.fee_percentage, 
    fa.fee_amount, 
    fa.commission_percentage, 
    fa.commission_amount, 
    fa.display_on_quotes, 
    fa.party_cnt,
    fa.extra_scheme_id,
    fa.extra_amount_basis
FROM
    Fee_Amounts  AS fa, 
    risk_group   AS rg, 
    PMCaption    AS pc, 
    party        AS p
WHERE   
    fa.risk_group_id = rg.risk_group_id
AND     
    rg.caption_id = pc.caption_id
AND     
    pc.language_id = @language_id
AND     
    p.party_cnt = fa.party_cnt
AND     
    rg.code = @code

GO


