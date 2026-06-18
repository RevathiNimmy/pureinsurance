SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_gis_report_1'
GO


CREATE PROCEDURE spu_gis_report_1
    @gis_action integer,
    @gis_day integer,
    @gis_month integer,
    @gis_year integer,
    @gis_insurer_id integer
AS


begin

SELECT

     count(*)

FROM
    GIS_Scheme_Audit GSA,
    GIS_Scheme GS,
    GIS_Insurer Ins
WHERE
    GSA.gis_scheme_id = GS.gis_scheme_id
    AND GSA.action = @gis_action
    AND day(GSA.Datetime_of_action) = @gis_day
    AND month(GSA.Datetime_of_action) = @gis_month
    AND year(GSA.Datetime_of_action) = @gis_year
    AND GS.gis_insurer_id = Ins.gis_insurer_id
    and @gis_insurer_id = Ins.gis_insurer_id

End
GO


