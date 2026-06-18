SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GIS_Select_Sch_Pay'
GO


CREATE PROCEDURE spu_GIS_Select_Sch_Pay
    @gis_scheme_id int
AS


SELECT p.code,
 p.description
FROM    GIIM_Scheme_Payment_Type s,
        GIIM_Payment_Type p
WHERE   s.GIIM_Payment_Type_id = p.GIIM_Payment_Type_id
AND     @gis_scheme_id = s.gis_scheme_id
ORDER BY p.Sequence_No
GO


