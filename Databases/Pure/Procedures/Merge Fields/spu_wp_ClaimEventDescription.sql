SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON  SET NOCOUNT ON
GO

EXECUTE DDLDropProcedure 'spu_wp_ClaimEventDescription'

GO

CREATE PROCEDURE spu_wp_ClaimEventDescription
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

    SELECT
        el.description EventDesc
    FROM Event_Log el
    WHERE el.claim_cnt = @ClaimCnt
    AND  el.event_type_id = 6
GO


SET NOCOUNT OFF

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO
