SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Renewal_Process_add'
GO

CREATE PROCEDURE spe_Renewal_Process_add
    @renewal_process_id int,
    @caption_id int,
    @code char(10),
    @description varchar(255),
    @is_deleted tinyint,
    @effective_date datetime
AS
BEGIN
INSERT INTO Renewal_Process (
    renewal_process_id ,
    caption_id ,
    code ,
    description ,
    is_deleted ,
    effective_date )
VALUES (
    @renewal_process_id,
    @caption_id,
    @code,
    @description,
    @is_deleted,
    @effective_date)
END

GO

