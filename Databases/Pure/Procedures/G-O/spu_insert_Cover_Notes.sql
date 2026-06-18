SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Insert_Cover_Notes'
GO
--Start (Sriram P) - (Tech Spec WR19 - Cover Note Functionality.doc)section 6.2.2 
CREATE PROCEDURE spu_Insert_Cover_Notes         
@Risk_Cover_Note_Link_Id  INT output,      
@Risk_Id   INT,      
@Cover_Note_Ref   VARCHAR(50) = null,      
@Cover_Note_From  DATETIME = null,      
@Cover_Note_To   DATETIME = null      
       
As     
      
   
     
INSERT INTO Risk_Cover_Note_Link (   
         Risk_Id,      
         Cover_Note_Ref,      
         Cover_Note_From,      
         Cover_Note_To)      
     VALUES (      
               
         @Risk_Id,      
         @Cover_Note_Ref,      
         @Cover_Note_From,      
         @Cover_Note_To)
select @Risk_Cover_Note_Link_Id=@@identity         

--End (Sriram P) - (Tech Spec WR19 - Cover Note Functionality.doc)section 6.2.2



SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO 