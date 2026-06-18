SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Ins_File_Lapse'
GO


CREATE PROCEDURE spu_Ins_File_Lapse
    @insurance_file_cnt int,
    @dtLapseDate smalldatetime
AS


DECLARE @iLapsedStatusID int
DECLARE @Insurance_Folder_Cnt int

    SELECT @iLapsedStatusID = ( SELECT insurance_file_status_id
                    FROM insurance_file_status
                    WHERE code = 'LAP')

    IF @dtLapseDate is null
        BEGIN

        UPDATE insurance_file
        SET insurance_file_status_id = @iLapsedStatusID
        WHERE insurance_file_cnt in (SELECT insurance_file.insurance_file_cnt
                        FROM    insurance_file,
                            insurance_file insurance_file_2,
                            insurance_file_type
                        WHERE insurance_file_2.insurance_file_cnt = @insurance_file_cnt
                        AND insurance_file.insurance_folder_cnt = insurance_file_2.insurance_folder_cnt
                        AND insurance_file.insurance_file_type_id = insurance_file_type.insurance_file_type_id
                        AND (insurance_file_type.code = 'MTAQUOTE' OR insurance_file_type.code = 'MTAQTETEMP'))
        END
    ELSE
        UPDATE insurance_file
        SET insurance_file_status_id = @iLapsedStatusID
        WHERE insurance_file_cnt in (SELECT insurance_file.insurance_file_cnt
                        FROM    insurance_file,
                            insurance_file insurance_file_2,
                            insurance_file_type
                        WHERE insurance_file_2.insurance_file_cnt = @insurance_file_cnt
                        AND insurance_file.insurance_folder_cnt = insurance_file_2.insurance_folder_cnt
                        AND insurance_file.insurance_file_type_id = insurance_file_type.insurance_file_type_id
                        AND (insurance_file_type.code = 'MTAQUOTE' OR insurance_file_type.code = 'MTAQTETEMP')
                        AND     insurance_file.cover_start_date >= @dtLapseDate
                        AND      insurance_file.cover_start_date <= GETDATE())
GO


