SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Analysis_Code_update'
GO


CREATE PROCEDURE spu_Analysis_Code_update
    @analysis_code_id smallint,
    @caption_id int,
    @code char(10),
    @description varchar(255),
    @is_deleted tinyint,
    @effective_date datetime
AS


/* Update the values */
UPDATE  Analysis_Code
SET caption_id = @caption_id,
    code = @code,
    description = @description,
    is_deleted = @is_deleted,
    effective_date = @effective_date
WHERE   analysis_code_id = @analysis_code_id
GO


