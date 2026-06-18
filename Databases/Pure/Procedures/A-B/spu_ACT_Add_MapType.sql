SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Add_MapType'
GO


CREATE PROCEDURE spu_ACT_Add_MapType
    @maptype_id smallint,
    @description varchar(255),
    @code char(10),
    @is_deleted bit,
    @effective_date datetime,
    @caption_id int
AS


BEGIN
INSERT INTO MapType (
    maptype_id ,
    description ,
    code ,
    is_deleted ,
    effective_date ,
    caption_id )
VALUES (
    @maptype_id,
    @description,
    @code,
    @is_deleted,
    @effective_date,
    @caption_id)
END
GO


