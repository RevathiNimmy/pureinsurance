SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_PFPaymentMethod_sellist'
GO
CREATE PROCEDURE spe_PFPaymentMethod_sellist
AS

SELECT
    PFPaymentMethod_cnt,
    Description
FROM
    PFPaymentMethod
ORDER BY
    PFPaymentMethod_cnt

GO

