SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Add_Write_Off_Reason'
GO


CREATE PROCEDURE spu_ACT_Add_Write_Off_Reason
    @write_off_reason_id int,
    @description varchar(255),
    @is_deleted tinyint,
    @code varchar(10),
    @caption_id int,
    @effective_date datetime
AS


BEGIN
INSERT INTO Write_Off_Reason (
    write_off_reason_id ,
    description ,
    is_deleted ,
    code ,
    caption_id ,
    effective_date )
VALUES (
    @write_off_reason_id,
    @description,
    @is_deleted,
    @code,
    @caption_id,
    @effective_date)
END
GO


