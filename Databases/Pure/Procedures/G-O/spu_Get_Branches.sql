
--Start (Arul Stephen) - (TechSpec WR6ClauseGrouping.doc)
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Get_Branches'
GO
CREATE procedure spu_Get_Branches    
    
as     
    
Select Source_id, code ,description from source where is_deleted=0 
and effective_date<=getdate()    
  
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO 
--End (Arul Stephen) - (TechSpec WR6ClauseGrouping.doc)