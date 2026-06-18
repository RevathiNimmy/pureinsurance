SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_copy_folder_to_event'
GO


CREATE PROCEDURE spu_copy_folder_to_event
    @event_cnt int,
    @insurance_folder_cnt int
AS


BEGIN
INSERT INTO event_insurance_folder (
    insurance_folder_cnt,
    insurance_folder_id,
    source_id,
    insurance_holder_cnt,
    code,
    description,
    inception_date,
    arc_archive_folder_id,
    quote_insurance_ref,
    next_insurance_ref,
    last_insurance_ref,
    renewal_count)
select @event_cnt,
    insurance_folder_id,
    source_id,
    insurance_holder_cnt,
    code,
    description,
    inception_date,
    arc_archive_folder_id,
    quote_insurance_ref,
    next_insurance_ref,
    last_insurance_ref,
    renewal_count
from    insurance_folder
where   insurance_folder_cnt = @insurance_folder_cnt
END
GO


