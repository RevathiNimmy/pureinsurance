SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_coinsurercount'
GO


CREATE PROCEDURE spu_wp_coinsurercount
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT = NULL,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

 DECLARE @AgentUnderwriter varchar(1)

 SELECT @AgentUnderwriter = value
        FROM Hidden_Options
        WHERE branch_id = 1 AND option_number = 1
    IF ISNULL(@AgentUnderwriter, '') = 'U' BEGIN
        SELECT @AgentUnderwriter = 'U'
    END

IF @AgentUnderwriter = 'U'
	SELECT	SUM(1) as how_many
	FROM	coi_value
	WHERE	insurance_file_cnt = @InsuranceFileCnt
ELSE
	SELECT	SUM(1) as how_many
	FROM	policy_coinsurers
	WHERE	insurance_file_cnt = @InsuranceFileCnt

GO

