
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_DT_Get_Party_Data'
GO

/*******************************************************************************************************/  
/* spu_SAM_DT_Get_Party_Data                                                           			*/  
/*                                                                                         	        */  
/* Gets the Party data from the Sirius DB for use in the DT Staging DB					*/  
/*******************************************************************************************************/  

Create Procedure spu_SAM_DT_Get_Party_Data
as
	SELECT p.Party_cnt, p.Party_type_id, p.ShortName, a.Account_Key, a.short_code
	FROM Party p
	LEFT JOIN Account a ON p.party_cnt = a.account_id
 	LEFT JOIN Party_Type pt ON p.Party_type_id = pt.party_type_id
	WHERE pt.Code = 'AG' OR pt.Code = 'IN'
	ORDER BY p.party_type_id

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

