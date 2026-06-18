SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Events'
GO

CREATE PROCEDURE spu_Report_Events
	@Start_Date datetime,
	@End_Date datetime,
	@Event_Type varchar(255),
	@Party_Code varchar(255)
AS

SET NOCOUNT ON

DECLARE	@SelectedEventType integer,
	@SelectedClient integer,
	@SelectedPolicy integer,
	@SelectedClaim integer,
	@EventTypeId integer,
	@EventDate datetime,
	@PartyCode varchar(50),
	@PartyName varchar(255),
	@PolicyNumber varchar(50),
	@ClaimNumber varchar(50),
	@Description varchar(255),
	@FSADisabled integer
	
SELECT @FSAdisabled = 0

IF NOT EXISTS 
    (
        SELECT NULL
        FROM hidden_options
        WHERE option_number = 61
        AND value = '1'
    )
BEGIN
    SELECT @FSADisabled = 1
END

IF @Event_Type = 'ALL'
BEGIN
	SELECT @SelectedEventType = 0
END
ELSE
BEGIN
	SELECT @SelectedEventType = ( SELECT event_type_id FROM event_type WHERE description = @Event_Type )
END

IF @Party_Code = 'ALL'
BEGIN
	SELECT @SelectedClient = 0
END
ELSE
BEGIN
	SELECT @SelectedClient = ( SELECT party_cnt FROM party WHERE shortname = @Party_Code )
END

CREATE TABLE #Report_Temp
(
	event_date		datetime,
	client_code		varchar(30),
	client_name		varchar(255),
	policy_number		varchar(50),
	claim_number		varchar(30),
	[description]		varchar(255)
)

DECLARE	c_cursor CURSOR FAST_FORWARD FOR
SELECT	el.event_type_id,
	el.event_date,
	p.shortname,
	p.resolved_name,
	i.insurance_ref,
	c.claim_number,
	el.[description]
FROM	event_log el
LEFT OUTER JOIN	party p
ON	el.party_cnt = p.party_cnt
LEFT OUTER JOIN	insurance_file i
ON	el.insurance_file_cnt = i.insurance_file_cnt
LEFT OUTER JOIN	claim c
ON	el.claim_cnt = c.claim_id
WHERE	el.event_date >= @Start_Date
AND	el.event_date <= @End_Date
AND	(@SelectedEventType = 0 OR
	el.event_type_id = @SelectedEventType)
AND	(@SelectedClient = 0 OR
	el.party_cnt = @SelectedClient)

OPEN	c_cursor

FETCH NEXT FROM c_cursor INTO	@EventTypeId,
				@EventDate,
				@PartyCode,
				@PartyName,
				@PolicyNumber,
				@ClaimNumber,
				@Description

WHILE @@FETCH_STATUS = 0
BEGIN

	IF @Description IS NULL or rtrim(@Description) = ''
	BEGIN
		SELECT @Description = (SELECT [description] FROM event_type WHERE event_type_id = @EventTypeId)		
	END

	INSERT INTO #Report_Temp
	SELECT @EventDate event_date,
		@PartyCode party_code,
		@PartyName party_name,
		@PolicyNumber policy_number,
		@ClaimNumber claim_number,
		@Description description

	FETCH NEXT FROM c_cursor INTO	@EventTypeId,
					@EventDate,
					@PartyCode,
					@PartyName,
					@PolicyNumber,
					@ClaimNumber,
					@Description

END

CLOSE 		c_cursor

DEALLOCATE	c_cursor

SET NOCOUNT OFF

SELECT 	event_date,
	client_code,
	client_name,
	policy_number,
	claim_number,
	[description],
	@FSADisabled 'fsa_disabled'
FROM	#Report_Temp
ORDER BY event_date, client_code, policy_number, claim_number

DROP TABLE #Report_Temp

GO
