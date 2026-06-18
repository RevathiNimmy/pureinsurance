SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Treaty_sel'
GO


CREATE PROCEDURE spu_Treaty_sel
    @treaty_id int
AS

    Select  t.treaty_id,
            t.code,
            t.description,
            t.is_deleted,
            t.effective_date,
            t.expiry_date,
            t.agreement_code,
            t.reinsurance_type_id,
            rt.description,
            t.replaces_treaty_id,
            r.description,
           (Select  Sum(commission_percent * share_percent / 100)
            From    treaty_party
            Where   treaty_id = t.treaty_id) avg_commission,
            Case When rt.code = 'RET' Then 1 Else 0 End is_retained
    From    Treaty t
    Left Join
            Reinsurance_Type rt
            On rt.reinsurance_type_id = t.reinsurance_type_id
    Left Join
            Treaty r
            On r.treaty_id = t.replaces_treaty_id
    Where   t.treaty_id = @treaty_id

GO

