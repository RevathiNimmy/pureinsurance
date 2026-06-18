SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_party_agent_upd'
GO


CREATE PROCEDURE spu_party_agent_upd
    @party_cnt int,
    @default_percent numeric
AS


BEGIN
UPDATE  party_agent
SET default_commission_percent = @default_percent
WHERE   party_cnt = @party_cnt
END
GO


