SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure spu_SIR_Rating_SectionsTypes_Sel  
GO

CREATE Procedure spu_SIR_Rating_SectionsTypes_Sel  
 @risk_type_id bigint,  
 @rating_section_type_id INT,
 @UserId INT = NULL,
 @UniqueId VARCHAR(50) = NULL,
 @ScreenHierarchy VARCHAR(500) = NULL   
AS  
  
Select rst.Rating_Section_Type_id,(Trim(rst.code)+' - '+ Trim(rst.description)) AS description,  
CASE When v.Rating_Section_Type_id IS NULL THEN 0 ELSE 1 END as Chosen  
From   rating_section_type rst left Join   
(   
Select DISTINCT rtrst.rating_section_type_id  
from Risk_Type_Rating_Section_Type rtrst   
where rtrst.risk_type_id=@risk_type_id  
) v   
on  
rst.rating_section_type_id=v.rating_section_type_id  
  
WHERE rst.effective_date <= getdate()  AND rst.is_deleted = 0   
ORDER BY description  
