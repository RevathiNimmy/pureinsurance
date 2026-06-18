SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Party_Agent_Group_sel'
GO

CREATE PROCEDURE spe_Party_Agent_Group_sel
    @party_cnt int
AS
SELECT
    party_cnt,
    active
 FROM Party_Agent_Group
WHERE party_cnt = @party_cnt

GO

