SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_CLM_party_Supplier_add'
GO

CREATE PROCEDURE spu_SAM_CLM_party_Supplier_add
    
@party_cnt int,    
@supplier_speciality_id int,    
@supplier_business_id int    
 
 
    
AS    
    
BEGIN    
 INSERT INTO party_Supplier_Business
(
party_cnt,
supplier_speciality_id,
supplier_business_id
)
VALUES
( 
@party_cnt,
@supplier_speciality_id,
@supplier_business_id 
)
END    


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
