SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Detach_Cover_Notes'
GO
--Start (Sriram P) - (Tech Spec WR19 - Cover Note Functionality.doc)section 6.2.4 
CREATE PROCEDURE spu_Detach_Cover_Notes   
    @Risk_Id int  
AS  
  
DELETE FROM Risk_Cover_Note_Link
WHERE Risk_Id = @Risk_Id 
GO
--End (Sriram P) - (Tech Spec WR19 - Cover Note Functionality.doc)section 6.2.4
GO


SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO