EXECUTE DDLDropProcedure 'spu_GIS_Add_Rate_Type'
GO


SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

--save new or update Rate Type
CREATE PROCEDURE spu_GIS_Add_Rate_Type

@SchemeID int,
@Description varchar(70),   
@ListType1 int,
@ListType2 int,
@ListType3 int,
@RateTypeID int

AS

IF @RateTypeID=0

    BEGIN

        INSERT INTO gis_rate_type
         (description,gis_scheme_id, gis_list_type_lookup1,gis_list_type_lookup2,gis_list_type_lookup3)
        VALUES (@description,@schemeid,@listtype1,@listtype2,@listtype3)

    END

ELSE

    BEGIN
        UPDATE gis_rate_type
        SET
        gis_list_type_lookup1=@listtype1,
        gis_list_type_lookup2=@listtype2,
        gis_list_type_lookup3=@listtype3
        WHERE gis_rate_type_id=@ratetypeid

    END
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO
