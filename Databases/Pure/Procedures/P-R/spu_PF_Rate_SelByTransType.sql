SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

DDLDropProcedure 'spu_PF_Rate_SelByTransType'
GO

-- ADO #39455: Instalment for Claim Recovery - Scheme Configuration
-- Select rates filtered by scheme and transaction_type
CREATE PROCEDURE spu_PF_Rate_SelByTransType
    @CompanyNo INT,
    @SchemeNo INT,
    @SchemeVersion INT,
    @TransactionType TINYINT = NULL
AS
BEGIN
    SELECT
        r.*
    FROM
        PFRF r
    WHERE
        r.CompanyNo = @CompanyNo
        AND r.SchemeNo = @SchemeNo
        AND r.SchemeVersion = @SchemeVersion
        AND (
            (@TransactionType IS NULL AND r.transaction_type IS NULL)
            OR r.transaction_type = @TransactionType
        )
END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
