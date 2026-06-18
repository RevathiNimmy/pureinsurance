SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Analysis_Code_add'
GO


CREATE PROCEDURE spu_Analysis_Code_add
    @analysis_code_id smallint,
    @caption_id int,
    @code char(10),
    @description varchar(255),
    @is_deleted tinyint,
    @effective_date datetime
AS


/* Insert the values */
INSERT INTO Analysis_Code
( analysis_code_id, caption_id, code, description, is_deleted, effective_date )
VALUES
( @analysis_code_id, @caption_id, @code, @description, @is_deleted, @effective_date )
GO


