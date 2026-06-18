SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Event_Premiums_sel'
GO


CREATE PROCEDURE spu_Event_Premiums_sel
    @insurance_file_cnt int
AS


SELECT
    insurance_file_cnt,
    party_cnt,
    Split_Value,
    Split_percentage
FROM Event_Policy_Shared_Premiums
WHERE insurance_file_cnt = @insurance_file_cnt
GO


