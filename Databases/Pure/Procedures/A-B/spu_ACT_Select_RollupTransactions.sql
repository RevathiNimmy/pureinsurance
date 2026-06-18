SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Select_RollupTransactions'
GO

CREATE PROCEDURE spu_ACT_Select_RollupTransactions
    @lDocumentId INT,
    @lAccountId INT = NULL

AS

-- FILTER ON ACCOUNT AS WELL IF PASSED
IF (@lAccountId IS NOT NULL) BEGIN
    SELECT
        transdetail_id
    FROM
        transdetail
    WHERE
        document_id = @lDocumentId
    AND
        account_id = @lAccountId
END
ELSE BEGIN
    SELECT
        transdetail_id
    FROM
        transdetail
    WHERE
        document_id = @lDocumentId

END
GO
