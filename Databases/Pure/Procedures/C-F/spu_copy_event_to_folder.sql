SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_copy_event_to_folder'
GO


CREATE PROCEDURE spu_copy_event_to_folder
    @event_cnt int,
    @insurance_folder_cnt int
AS


DECLARE
    @insurance_folder_id int  ,
    @source_id smallint ,
    @insurance_holder_cnt int  ,
    @code varchar (40)  ,
    @description varchar (255) ,
    @inception_date datetime ,
    @arc_archive_folder_id int  ,
    @quote_insurance_ref varchar (30)  ,
    @next_insurance_ref varchar (30)  ,
    @last_insurance_ref varchar (30)  ,
    @renewal_count int
BEGIN
SELECT  @insurance_folder_id=insurance_folder_id,
    @source_id=source_id,
    @insurance_holder_cnt=insurance_holder_cnt,
    @code=code,
    @description=description,
    @inception_date=inception_date,
    @arc_archive_folder_id=arc_archive_folder_id,
    @quote_insurance_ref=quote_insurance_ref,
    @next_insurance_ref=next_insurance_ref,
    @last_insurance_ref=last_insurance_ref,
    @renewal_count=renewal_count
FROM    event_insurance_folder
WHERE   insurance_folder_cnt = @event_cnt
UPDATE  insurance_folder SET
    insurance_folder_id=@insurance_folder_id,
    source_id=source_id,
    insurance_holder_cnt=@insurance_holder_cnt,
    code=@code,
    description=@description,
    inception_date=@inception_date,
    arc_archive_folder_id=@arc_archive_folder_id,
    quote_insurance_ref=@quote_insurance_ref,
    next_insurance_ref=@next_insurance_ref,
    last_insurance_ref=@last_insurance_ref,
    renewal_count=@renewal_count
WHERE   insurance_folder_cnt=@insurance_folder_cnt
END
GO


