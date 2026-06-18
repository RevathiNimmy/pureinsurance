EXECUTE DDLDropProcedure 'spu_PFScheme_GetCompanyNo'
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE spu_PFScheme_GetCompanyNo

@party_cnt int

AS BEGIN

    SELECT finance_provider_number 
    FROM Party_Finance_Provider 
    WHERE party_cnt= @party_cnt

END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO