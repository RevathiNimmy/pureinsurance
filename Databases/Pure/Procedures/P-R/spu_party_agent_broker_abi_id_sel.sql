SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_party_agent_broker_abi_id_sel'
GO

CREATE PROCEDURE spu_party_agent_broker_abi_id_sel
    @broker_abi_id varchar(20)
   
AS
BEGIN
    SELECT party_cnt,trading_name
        FROM party_agent
        WHERE broker_abi_id = @broker_abi_id

END


GO


