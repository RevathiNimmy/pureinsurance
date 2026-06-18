--Arul Bug fixing - PN 55153
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Get_Clauses_Set_To_default_In_Product'
GO
CREATE PROCEDURE  spu_Get_Clauses_Set_To_default_In_Product
     
@product_id int,
@Source_id int    
     
AS  

SELECT  dt.code, dt.description, dt.document_template_id  
FROM wording_product_link wpl
 inner join document_template dt on dt.document_template_id = wpl.document_template_id

 WHERE wpl.branch_id =@Source_id and  wpl.product_id=@product_id  and wpl.[default]=1 

 GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO 
--End Arul Bug fixing - PN 55153