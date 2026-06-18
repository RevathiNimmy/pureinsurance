SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_gis_report_3'
GO


CREATE PROCEDURE spu_gis_report_3
    @gis_day integer,
    @gis_month integer,
    @gis_year integer,
    @gis_insurer_id integer
AS


begin

SELECT DISTINCT
    GS.scheme_desc Scheme_Name,
    Datetime_of_action Datetime,
    Action = GSA.action

FROM
    GIS_Scheme_Audit GSA,
    GIS_Scheme GS,
    GIS_Insurer Ins
WHERE
    GSA.gis_scheme_id = GS.gis_scheme_id
    AND ( (GSA.action = 0) or (GSA.action = 1) or (GSA.action = 2) or (GSA.action = 3) )
    AND day(GSA.Datetime_of_action) = @gis_day
    AND month(GSA.Datetime_of_action) = @gis_month
    AND year(GSA.Datetime_of_action) = @gis_year
    AND GS.gis_insurer_id = Ins.gis_insurer_id
    and @gis_insurer_id = Ins.gis_insurer_id

ORDER BY  Datetime_of_action

End
GO


