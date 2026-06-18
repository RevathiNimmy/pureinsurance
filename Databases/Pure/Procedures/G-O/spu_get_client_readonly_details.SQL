SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_get_client_readonly_details'
GO
Create PROCEDURE spu_get_client_readonly_details 
    @code VARCHAR(10)  
AS  
	
	SELECT pt.code,ns.party_type_id,ISNULL(ns.is_read_only,0) AS is_read_only,
	ns.mask_code
	FROM party_type pt 
	INNER JOIN numbering_scheme ns
	ON pt.party_type_id=ns.party_type_id
	
	WHERE ns.is_deleted <> 1
	AND ns.party_type_id > 0
	AND @code=pt.code

GO