SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_TP_coins_details'
GO


CREATE PROCEDURE spu_get_TP_coins_details
    @ClaimID INT
AS


DECLARE @AgentUnderwriter varchar(1)

SELECT  @AgentUnderwriter = value
FROM    hidden_options
WHERE   branch_id = 1 and option_number = 1

IF @AgentUnderwriter is null
    SELECT @AgentUnderwriter = "A"

IF @AgentUnderwriter = ""
    SELECT @AgentUnderwriter = "A"

IF @AgentUnderwriter = "A"
    Select  P.Party_cnt,
        P.name,
        CP.Share,
        CP.Share_Value
    from    Claim_Party CP,
        Party P
    where   CP.Claim_id = @ClaimID
    AND CP.Party_id = P.Party_cnt
    AND insurer_type=0
ELSE

    Select  P.Party_cnt,
        P.name,
        CP.Share,
        CP.Share_Value
    from    Work_Claim_Party CP,
        Party P
    where   CP.Claim_id = @ClaimID
    AND CP.Party_id = P.Party_cnt
    AND insurer_type=0
GO


