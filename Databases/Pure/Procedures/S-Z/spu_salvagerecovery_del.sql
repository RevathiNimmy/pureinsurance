SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_salvagerecovery_del'
GO


CREATE PROCEDURE spu_salvagerecovery_del
    @recovery_id int
AS

--*******************************************************************************************
-- Version      Author  Date        Desc
-- 1.00.0001    TN      27/04/2001  process data from work tables for underwriting
--
--*******************************************************************************************
DECLARE @AgentUnderwriter varchar(1)

SELECT  @AgentUnderwriter = value
FROM    hidden_options
WHERE   branch_id = 1 and option_number = 1

IF @AgentUnderwriter is null
    SELECT @AgentUnderwriter = "A"

IF @AgentUnderwriter = ""
    SELECT @AgentUnderwriter = "A"

IF @AgentUnderwriter = "A"
    DELETE FROM recovery
    WHERE recovery_id = @recovery_id
ELSE
--UNDERWRITING
    DELETE FROM work_recovery
    WHERE recovery_id = @recovery_id
GO


