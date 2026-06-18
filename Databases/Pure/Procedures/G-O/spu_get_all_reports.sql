EXECUTE DDLDropProcedure 'spu_get_all_reports'
GO
SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE spu_get_all_reports
AS
BEGIN

select report_id,description
from report
order by description

END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

