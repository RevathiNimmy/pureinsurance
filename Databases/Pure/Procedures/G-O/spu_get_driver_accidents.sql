SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_driver_accidents'
GO


CREATE PROCEDURE spu_get_driver_accidents
    @PartyID int
AS


SELECT previous_accidents_id,
        date,
        description,
        is_at_fault
    FROM previous_accidents
    WHERE party_cnt = @PartyID
GO


