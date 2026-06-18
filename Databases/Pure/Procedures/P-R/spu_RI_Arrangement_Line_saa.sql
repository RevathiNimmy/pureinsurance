SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_RI_Arrangement_Line_saa'
GO


CREATE PROCEDURE spu_RI_Arrangement_Line_saa
    @ri_arrangement_id int
AS

    Select  -- RI Grid fields
            Case When type = 'F' Then p.resolved_name Else t.description End [ri_name],
            ral.default_share_percent / 100,
            ral.this_share_percent / 100,
            ral.sum_insured,
            ral.premium_value,
            ral.premium_tax,
            ral.commission_percent / 100,
            ral.commission_value,
            ral.commission_tax,
            ral.agreement_code,
            -- Supporting fields
            ral.ri_arrangement_line_id,
            ral.type,
            ral.treaty_id,
            ral.party_cnt,
            ral.priority,
            ral.number_of_lines,
            ral.line_limit,
            ral.premium_percent / 100,
	    --Start-(Arul Stephen)-(WPR2 - Reinsurance Obligatory)  
            ral.is_commission_modified,  
	    ral.is_obligatory,
            t.code treatycode  
            --End-(Arul Stephen)-(WPR2 - Reinsurance Obligatory)  
    From    RI_Arrangement_Line ral
    Left Join
            Treaty t
            On t.treaty_id = ral.treaty_id
    Left Join
            Reinsurance_type rt
            On rt.reinsurance_type_id = t.reinsurance_type_id
    Left Join
            Party p
            On p.party_cnt = ral.party_cnt
    Where   ri_arrangement_id = @ri_arrangement_id 
    Order By
            ri_arrangement_id, priority, default_share_percent
    

Go



