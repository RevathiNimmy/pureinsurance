SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


-- this will return all events for this policy
-- user will have to use if statements in conjuction with event type and description
-- to filter out relevant events

EXECUTE DDLDropProcedure 'spu_wp_PolicyEventCount'
GO


CREATE PROCEDURE spu_wp_PolicyEventCount
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

SELECT 	COUNT(1)
FROM 	Event_Log el JOIN Event_Type et ON el.event_type_id = et.event_type_id
WHERE insurance_file_cnt = @InsuranceFileCnt

GO

