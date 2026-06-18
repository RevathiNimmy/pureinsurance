SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_copy_event_to_system'
GO


CREATE PROCEDURE spu_copy_event_to_system
    @event_cnt int,
    @insurance_file_cnt int
AS


DECLARE
    @endorsement_count int    ,
    @created_by_id smallint    ,
    @date_created datetime    ,
    @modified_by_id smallint    ,
    @last_modified datetime    ,
    @last_trans_date datetime    ,
    @last_trans_type_id int    ,
    @last_trans_description varchar (255)    ,
    @last_trans_debit_credit char (1)    ,
    @last_trans_document_ref varchar (25)    ,
    @last_trans_cover_start_date datetime    ,
    @last_trans_expiry_date datetime
BEGIN
SELECT  @endorsement_count=endorsement_count,
    @created_by_id=created_by_id,
    @date_created=date_created,
    @modified_by_id=modified_by_id,
    @last_modified=last_modified,
    @last_trans_date=last_trans_date,
    @last_trans_type_id=last_trans_type_id,
    @last_trans_description=last_trans_description,
    @last_trans_debit_credit=last_trans_debit_credit,
    @last_trans_document_ref=last_trans_document_ref,
    @last_trans_cover_start_date=last_trans_cover_start_date,
    @last_trans_expiry_date=last_trans_expiry_date
FROM    event_insurance_file_system
WHERE   insurance_file_cnt = @event_cnt

UPDATE  insurance_file_system SET
    endorsement_count=@endorsement_count,
    created_by_id=@created_by_id,
    date_created=@date_created,
    modified_by_id=@modified_by_id,
    last_modified=@last_modified,
    last_trans_date=@last_trans_date,
    last_trans_type_id=@last_trans_type_id,
    last_trans_description=@last_trans_description,
    last_trans_debit_credit=@last_trans_debit_credit,
    last_trans_document_ref=@last_trans_document_ref,
    last_trans_cover_start_date=@last_trans_cover_start_date,
    last_trans_expiry_date=@last_trans_expiry_date
WHERE   insurance_file_cnt=@insurance_file_cnt
END
GO


