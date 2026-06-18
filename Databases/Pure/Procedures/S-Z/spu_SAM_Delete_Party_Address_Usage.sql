SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Delete_Party_Address_Usage'
GO

CREATE PROCEDURE spu_SAM_Delete_Party_Address_Usage

@party_cnt int

AS

DELETE 
FROM party_address_usage 
WHERE party_cnt = @party_cnt



GO
