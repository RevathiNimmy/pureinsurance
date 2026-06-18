SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Get_Party_Supplier_Business'
GO

CREATE PROCEDURE spu_SAM_Get_Party_Supplier_Business

@party_cnt integer

AS

SELECT sb.code AS business_code, ss.code AS speciality_code 
FROM party_supplier_business psb

LEFT OUTER JOIN supplier_speciality ss ON 
	ss.supplier_speciality_id = psb.supplier_speciality_id 

LEFT OUTER JOIN supplier_business sb ON
	sb.supplier_business_id = psb.supplier_business_id

WHERE party_cnt = @party_cnt


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
