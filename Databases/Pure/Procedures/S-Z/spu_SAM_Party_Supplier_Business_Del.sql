SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Party_Supplier_Business_Del'
GO

CREATE PROCEDURE spu_SAM_Party_Supplier_Business_Del

@party_cnt int

AS

DELETE FROM party_supplier_business
WHERE party_cnt = @party_cnt


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
