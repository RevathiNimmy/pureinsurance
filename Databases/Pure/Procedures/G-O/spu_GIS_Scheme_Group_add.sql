SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GIS_Scheme_Group_add'
GO


CREATE PROCEDURE spu_GIS_Scheme_Group_add
    @code varchar(10),
    @caption_id int,
    @description varchar(255),
    @gis_business_type_id int,
    @gis_scheme_group_id int OUTPUT
AS


DECLARE @GroupID int
SELECT @GroupID = (SELECT MAX(gis_scheme_group_id) FROM GIS_Scheme_Group)  + 1
if @GroupID = NULL
    SELECT  @GroupID = 1
INSERT INTO GIS_Scheme_Group
    ( gis_scheme_group_id,
      code,
      caption_id,
      description,
      is_deleted,
      effective_date,
      gis_business_type_id )
    VALUES
    ( @GroupID,
      @code,
      @caption_id,
      @description,
      0,
      GETDATE(),
      @gis_business_type_id )
IF @@ERROR = 0

    BEGIN
        SELECT @gis_scheme_group_id = @GroupID
    END
GO


