SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

-- user will have to use if statements in conjuction with event type and description
-- to filter out relevant events

EXECUTE DDLDropProcedure 'spu_wp_PolicyEvent'
GO


CREATE PROCEDURE spu_wp_PolicyEvent
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

DECLARE
    @EventTypeCode VARCHAR(10), 
    @EventTypeDesc VARCHAR(255), 
    @EventDate DATETIME, 
    @EventDesc VARCHAR(1000),
    @EventUserName VARCHAR(255)

DECLARE PolicyEvent_Cursor SCROLL CURSOR FOR  
    SELECT  
        et.code,  
        et.description,  
        el.event_date,  
        el.description,  
        u.username
    FROM Event_Log el  
    JOIN Event_Type et  
        ON el.event_type_id = et.event_type_id
    JOIN Pmuser u
	ON u.user_id = el.user_id 
    WHERE insurance_file_cnt = @InsuranceFileCnt  
    ORDER BY el.event_cnt  
  
OPEN PolicyEvent_Cursor  
  
FETCH ABSOLUTE @Instance1 FROM PolicyEvent_Cursor INTO  
    @EventTypeCode,  
    @EventTypeDesc,  
    @EventDate,  
    @EventDesc,  
    @EventUserName
CLOSE PolicyEvent_Cursor  
DEALLOCATE PolicyEvent_Cursor  
  
SELECT  
    @EventTypeCode 'EventTypeCode',  
    @EventTypeDesc 'EventTypeDesc',  
    @EventDate 'EventDate',  
    @EventDesc 'EventDesc', 
    @EventUserName 'EventUserName'

GO

