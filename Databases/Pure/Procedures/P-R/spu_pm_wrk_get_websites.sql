SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_pm_wrk_get_websites'
GO

CREATE PROCEDURE spu_pm_wrk_get_websites
AS
BEGIN

SELECT PMW.code, PMW.description, WMI.code, PMW.website_url, PMW.button_tooltip
FROM pmwrk_websites PMW
LEFT OUTER JOIN Work_Manager_Icon WMI
ON PMW.Work_Manager_Icon_Id=WMI.Work_Manager_Icon_Id
WHERE pmw.is_deleted <> 1
ORDER BY pmwrk_websites_id

END
GO