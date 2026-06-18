SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS OFF

GO

EXECUTE Ddldropprocedure 'spu_Treaty_Party_saa'

GO

CREATE PROCEDURE Spu_treaty_party_saa
  @treaty_id INT
AS
  SELECT tp.treaty_party_id,
         tp.party_cnt,
         p.Resolved_Name,--PN71084
         tp.treaty_id,
         tp.share_percent,
         tp.commission_percent,
         Isnull(p.domiciled_for_tax, 0) 'domiciled_for_tax',
         Isnull(tg.tax_group_id, 0) 'tax_group_id',
         IsNull(tg.description, '') 'tax_group_descr',
	    --E016
		ISNULL(tp.Is_Reinsurer_Approved,0) 'Is_Reinsurer_Approved',
		tp.treaty_party_id   
  FROM   treaty_party tp
         JOIN Treaty t
           ON t.treaty_id = tp.treaty_id
         JOIN party p
           ON p.party_cnt = tp.party_cnt
         LEFT JOIN party_insurer i
           ON i.party_cnt = p.party_cnt
         LEFT JOIN tax_group tg
           ON tg.tax_group_id = i.tax_group_id
  WHERE  tp.treaty_id = @treaty_id
  ORDER  BY p.resolved_name

GO 
