SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Get_party_corrospondance_address_cnt'
GO

CREATE PROCEDURE spu_Get_party_corrospondance_address_cnt
@party_cnt int
AS
SELECT address_cnt 
FROM Party_Address_Usage 
WHERE address_usage_type_id=4 AND PARTY_CNT=@party_cnt
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO