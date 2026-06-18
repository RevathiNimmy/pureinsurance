SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Act_GetSpare_UW'
GO


---------------------------------------------------------------------------------------------------------------------------
-- Name : Thinh Nguyen
--
-- Desc : get value in spare field for this transdetail_id
--
----------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE spu_Act_GetSpare_UW
    @TransDetailID int
AS

BEGIN

    SELECT spare FROM TransDetail WHERE transdetail_id = @TransDetailID
END
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON
GO
