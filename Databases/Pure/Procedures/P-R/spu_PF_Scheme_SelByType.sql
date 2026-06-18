SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

DDLDropProcedure 'spu_PF_Scheme_SelByType'
GO

-- ADO #39455: Instalment for Claim Recovery - Scheme Configuration
-- Select schemes filtered by company and scheme_type
CREATE PROCEDURE spu_PF_Scheme_SelByType
    @CompanyNo INT,
    @SchemeType TINYINT = 1
AS
BEGIN
    SELECT
        s.CompanyNo,
        s.SchemeNo,
        s.SchemeVersion,
        s.SchemeName,
        s.SchemeDescription,
        s.StartDate,
        s.EndDate,
        s.scheme_type
    FROM
        PFScheme s
    WHERE
        s.CompanyNo = @CompanyNo
        AND s.scheme_type = @SchemeType
    ORDER BY
        s.SchemeNo ASC,
        s.SchemeVersion ASC
END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
