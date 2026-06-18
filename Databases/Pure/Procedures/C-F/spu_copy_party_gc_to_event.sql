SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_copy_party_gc_to_event'
GO


CREATE PROCEDURE spu_copy_party_gc_to_event
    @event_cnt int,
    @party_cnt int
AS


BEGIN
INSERT INTO event_party_group_client (
    party_cnt,
    party_group_type_id,
    is_registered_charity,
    charity_number,
    number_of_members)
select @event_cnt,

    party_group_type_id,
    is_registered_charity,
    charity_number,
    number_of_members
from    party_group_client
where   party_cnt = @party_cnt
END
GO


