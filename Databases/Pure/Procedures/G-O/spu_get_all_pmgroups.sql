EXECUTE DDLDropProcedure 'spu_get_all_pmgroups'
GO
SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE spu_get_all_pmgroups
AS
BEGIN

select pmuser_group_id,description
from pmuser_group
order by description

END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

