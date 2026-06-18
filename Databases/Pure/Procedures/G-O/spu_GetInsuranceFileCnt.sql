SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GetInsuranceFileCnt'
GO


CREATE PROCEDURE spu_GetInsuranceFileCnt
    @TransDetailID INT
AS


SELECT D.insurance_file_cnt
FROM   TransDetail TD
JOIN   Document D ON TD.Document_id = D.Document_id
WHERE  TD.TransDetail_id = @TransDetailID


GO


