--*************************************
--Developed By : Puneet Kukreti
--Description : Navigator Enhancement 
--Dated:16/08/2006 
--*************************************
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_getDocumentsFlag '
GO

CREATE PROCEDURE spu_getDocumentsFlag   
	@product_id int  
AS  
SELECT 
	produce_schedule, 
	produce_certificate, 
	produce_debit_note 
FROM product 
WHERE product_id=@product_id and is_deleted=0

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
