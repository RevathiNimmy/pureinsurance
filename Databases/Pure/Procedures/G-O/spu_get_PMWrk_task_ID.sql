
--Start( Saurabh Agrawal )Tech Spec - PGR005 Automated Email(5.1.2.7)

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


EXECUTE DDLDropProcedure 'spu_get_PMWrk_task_ID'
GO
Create  Procedure spu_get_PMWrk_task_ID
	@taskcode varchar(20)
As
BEGIN
	select 	pmwrk_task_id
	from 	pmwrk_task
	where 	code = @taskcode
END



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

--End( Saurabh Agrawal )Tech Spec - PGR005 Automated Email(5.1.2.7)