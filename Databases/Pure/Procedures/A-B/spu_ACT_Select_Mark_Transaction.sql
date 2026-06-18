SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Select_Mark_Transaction'
GO

CREATE PROCEDURE spu_ACT_Select_Mark_Transaction
    @lDocumentId INT,  
    @lTransDetailId INT,  
    @lAccountId INT  
	
AS  
  
BEGIN 

DECLARE @SPartyCode as VARCHAR(10)

SELECT @SPartyCode=code
FROM party_type PT
JOIN 
   Party P ON p.party_type_id=PT.party_type_id
JOIN 
   Account A ON A.Account_key=P.party_cnt
WHERE
   A.Account_Id=@lAccountId	

IF (@SPartyCode='SU')
BEGIN
	SELECT 
	     transdetail_id 
	FROM 
	     TransMatch 
	WHERE 
	     transdetail_id=@lTransDetailId
END 
ELSE
BEGIN
	SELECT 
	    TM.transdetail_id
	FROM
	    TransMatch TM
	JOIN 
	    TransDetail TD ON(TD.transdetail_id=TM.transdetail_id)
	JOIN 
	    Account AC ON (AC.account_id=TD.account_id)
	JOIN 
	    Insurance_file INF ON (INF.lead_insurer_cnt=AC.account_key)
	JOIN 
	    Document DC ON (DC.insurance_file_cnt=INF.insurance_file_cnt)
	WHERE 
	    DC.document_id=@lDocumentId 
	    AND TD.document_id=@lDocumentId
END

END  

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO





