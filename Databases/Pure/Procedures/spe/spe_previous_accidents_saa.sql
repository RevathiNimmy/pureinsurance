SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_previous_accidents_saa'
GO

CREATE PROCEDURE spe_previous_accidents_saa
    @party_cnt int
AS

SELECT
    party_cnt,
    previous_accidents_id,
    Date,
    Description,
    is_at_fault
 FROM previous_accidents

WHERE party_cnt = @party_cnt
ORDER BY previous_accidents_id ASC

GO

