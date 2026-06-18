SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

Execute DDLDropProcedure 'spu_SAM_Claim_ReceiptPartyType'
GO

CREATE PROCEDURE spu_SAM_Claim_ReceiptPartyType  
@ReceiptPartykey int, 

@count int OUTPUT  
AS  

SELECT @count=count(*) FROM party p  
JOIN Party_type pt on  
pt.party_type_id=p.party_type_id  

WHERE p.party_cnt = @ReceiptPartykey and pt.code in ('PC','GC','CC')


GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
