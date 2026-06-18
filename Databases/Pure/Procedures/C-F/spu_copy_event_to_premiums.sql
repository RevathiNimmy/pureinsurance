SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_copy_event_to_premiums'
GO


CREATE PROCEDURE spu_copy_event_to_premiums
    @event_cnt int,
    @insurance_file_cnt int
AS


BEGIN
DELETE FROM
    policy_shared_premiums
WHERE   insurance_file_cnt = @insurance_file_cnt

INSERT INTO policy_shared_premiums (
    insurance_file_cnt,

    party_cnt,
    Split_Value,
    Split_percentage)
select @insurance_file_cnt,
    party_cnt,
    Split_Value,
    Split_percentage
from    event_policy_shared_premiums
where   insurance_file_cnt = @event_cnt
END
GO


