EXECUTE DDLDropProcedure 'spu_GIS_Save_Rate'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO


CREATE PROCEDURE spu_GIS_Save_Rate

@schemeID int,
@RateType varchar(70),
@Lookup1 int,
@Lookup2 int,
@Lookup3 int,
@Rate numeric(19,4)

AS

--get rate type  id

DECLARE @RateTypeID int

SELECT @RateTypeID=gis_rate_type_id from gis_rate_type
WHERE description=@RateType
AND Gis_Scheme_ID=@SchemeID


IF @Lookup3 is null

    Begin

        IF @Lookup2 is null
            Begin
                --1d
                UPDATE Gis_rate_items set rate=@rate
                WHERE gis_rate_Type_id=@RateTypeID
                AND Lookup1=@lookup1
            End
        Else
            Begin
                --2d
                UPDATE Gis_rate_items set rate=@rate
                WHERE gis_rate_Type_id=@RateTypeID
                AND Lookup1=@lookup1
                AND Lookup2=@lookup2
            End
    End
Else
    --3d
    Begin
        UPDATE Gis_rate_items set rate=@rate
        WHERE gis_rate_Type_id=@RateTypeID
        AND Lookup1=@lookup1
        AND Lookup2=@lookup2
        AND Lookup3=@lookup3
    End


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO

