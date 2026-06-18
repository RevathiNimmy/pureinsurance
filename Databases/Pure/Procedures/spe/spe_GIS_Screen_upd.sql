SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_GIS_Screen_upd'
GO

CREATE PROCEDURE spe_GIS_Screen_upd
    @GIS_screen_id int,
    @GIS_data_model_id int,
    @caption_id int,
    @code char(10),
    @description varchar(255),
    @is_deleted tinyint,
    @effective_date datetime,
    @parent_id int,
    @is_maintainable tinyint

AS
BEGIN

UPDATE GIS_Screen
    SET

    GIS_data_model_id=@GIS_data_model_id,
    caption_id=@caption_id,
    code=@code,
    description=@description,
    is_deleted=@is_deleted,
    effective_date=@effective_date,
    parent_id=@parent_id,
    is_maintainable=@is_maintainable

WHERE GIS_screen_id = @GIS_screen_id

END

GO

