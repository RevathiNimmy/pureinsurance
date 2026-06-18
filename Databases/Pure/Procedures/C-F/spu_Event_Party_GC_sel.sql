SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Event_Party_GC_sel'
GO


CREATE PROCEDURE spu_Event_Party_GC_sel
    @party_cnt int
AS


SELECT
    party_cnt,
    party_group_type_id,
    is_registered_charity
 FROM Event_Party_Group_Client
WHERE party_cnt = @party_cnt
GO


