SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_GetPartySupplier_details'
GO
--********************************************************************************************************************************  
-- VKG  11/09/2006        Get Party Supplier Details for other party
--*********************************************************************************************************************************  
CREATE PROCEDURE spu_SAM_GetPartySupplier_details 
@v_lPartyCnt int 
AS  
BEGIN 

SELECT * FROM Party_Supplier_Business 
WHERE party_cnt = @v_lPartyCnt
ORDER BY supplier_business_id
END

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
