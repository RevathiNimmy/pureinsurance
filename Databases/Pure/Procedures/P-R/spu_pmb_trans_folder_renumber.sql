SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pmb_trans_folder_renumber'
GO


CREATE PROCEDURE spu_pmb_trans_folder_renumber
    @folder_cnt int,
    @event_cnt int,
    @insurancefile_cnt int
AS


BEGIN
UPDATE transaction_export_folder
    SET
    insurance_file_cnt=@insurancefile_cnt
WHERE insurance_file_cnt = @event_cnt
END
GO


