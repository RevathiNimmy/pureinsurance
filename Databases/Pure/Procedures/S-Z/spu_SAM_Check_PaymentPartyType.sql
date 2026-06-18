SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

Execute DDLDropProcedure 'spu_SAM_Check_PaymentPartyType'  
GO

CREATE  PROCEDURE spu_SAM_Check_PaymentPartyType
 
@PartyKey int, 
@PartyType VARCHAR(10), 
@Valid int OUTPUT  
AS  

IF @PartyType = 'AGENT'
	BEGIN
		SELECT @Valid = count(Party_cnt) FROM Party p  
		JOIN Party_type pt on  
		pt.party_type_id=p.party_type_id  
		WHERE p.party_cnt = @Partykey and pt.code like 'AG%'
	END
IF @PartyType = 'PARTY'
	BEGIN
		SELECT @Valid = count(Party_cnt) FROM Party p  
		JOIN Party_type pt on  
		pt.party_type_id=p.party_type_id  
		WHERE p.party_cnt = @Partykey and (pt.code like 'OT%' OR pt.code like 'IN%' OR  pt.code IN ('PC','GC','CC'))
	END
IF @PartyType = 'CLIENT'
	BEGIN
		SELECT @Valid=count(Party_cnt) FROM Party p  
		JOIN Party_type pt on  
		pt.party_type_id=p.party_type_id  
		WHERE p.party_cnt = @Partykey and pt.code IN ('PC','GC','CC')
	END

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

--select * from party_type
--exec spu_SAM_Check_PaymentPartyType 14,'PARTY',NULL