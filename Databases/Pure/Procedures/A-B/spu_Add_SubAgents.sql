SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Add_SubAgents'
GO


CREATE PROCEDURE spu_Add_SubAgents
    @insurance_file_cnt int,
    @party_cnt int,
    @percentage numeric(7,4),
    @amount numeric(19,4)
AS


INSERT INTO insurance_file_agent (
    insurance_file_cnt,
    party_cnt,
    percentage,
    amount)
VALUES (
    @insurance_file_cnt,
    @party_cnt,
    @percentage,
    @amount)
GO


