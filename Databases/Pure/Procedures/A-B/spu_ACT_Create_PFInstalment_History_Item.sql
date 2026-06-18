SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Create_PFInstalment_History_Item'
GO

CREATE PROCEDURE spu_ACT_Create_PFInstalment_History_Item

@pfinstalments_id int

AS

BEGIN

	INSERT INTO PFInstalments_History
	(pfinstalments_id, 
	 posted_date, 
	 pfinstalments_status_id, 
	 pfinstalments_result_id) 

	SELECT 
		pfinstalments_id, 
		posteddate, 
		status, 
		pfinstalments_result_id
	FROM pfinstalments
	WHERE pfinstalments_id = @pfinstalments_id

END


GO
