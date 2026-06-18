SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Party_Supplier_Business_Add'
GO

CREATE PROCEDURE spu_SAM_Party_Supplier_Business_Add  
  
@party_cnt int,  
@supplier_business_id int,   
@supplier_speciality_id int  
  
AS  
  
INSERT INTO party_supplier_business  
(party_cnt, supplier_business_id,supplier_speciality_id)  
VALUES (@party_cnt,@supplier_business_id,@supplier_speciality_id)  


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
