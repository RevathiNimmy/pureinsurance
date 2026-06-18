SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_get_reports_limit_by_report_group'
GO

CREATE PROCEDURE spu_get_reports_limit_by_report_group  
    @report_group_code varchar(10)  
AS  

SELECT DISTINCT r.report_name, r.description
FROM 
    report r,  
    report_group rg,
    report_group_contents rgc
-- Join Tables...
WHERE 
    r.report_id = rgc.report_id 
AND
    rg.report_group_id = rgc.report_group_id
-- Apply Filters...
AND
    r.is_deleted = 0  
AND 
    datediff(day, r.effective_date, getdate()) >= 0  
AND 
    rg.is_deleted = 0  
AND 
    datediff(day, rg.effective_date, getdate()) >= 0  
AND
    rg.code like @report_group_code

GO