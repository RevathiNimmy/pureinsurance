--Start(saurabh Agrawal) Tech Spec LOA008 Account Handlers(6.1.2.1)

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Get_Party_Handler_Branch_List'
GO

CREATE  PROCEDURE spu_Get_Party_Handler_Branch_List 
 @party_cnt INT

AS
BEGIN
	SELECT 
		source_id,
		description,
		0 
	FROM 
		source 
	WHERE 
		is_deleted = 0 AND source_id not in 
		(SELECT source_id FROM party_handler_branch where party_cnt = @party_cnt  )

	UNION ALL 
		SELECT 
			phb.source_id,
			s.description,
			1
		FROM 
			party_handler_branch phb
		LEFT JOIN 
			source s ON phb.source_id = s.source_id
		WHERE 
			phb.party_cnt = @party_cnt and s.is_deleted = 0
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

--End(saurabh Agrawal) Tech Spec LOA008 Account Handlers(6.1.2.1)