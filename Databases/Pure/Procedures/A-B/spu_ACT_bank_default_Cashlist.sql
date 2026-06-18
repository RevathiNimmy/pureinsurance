SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_bank_default_Cashlist'
GO


CREATE PROCEDURE spu_ACT_bank_default_Cashlist    
    @source_id int,    
    @cashlisttype_id int    
AS    
SELECT    
    bankaccount_id,   
    mediatype_id   
FROM BankAccount_Default    
WHERE source_id = @source_id    
AND cashlisttype_id = @cashlisttype_id    
AND is_deleted <> 1



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
