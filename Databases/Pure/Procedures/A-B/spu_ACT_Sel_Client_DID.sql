SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Sel_Client_DID'
GO

CREATE PROCEDURE spu_ACT_Sel_Client_DID
    @transdetail_id int, 
    @true char(1) OUTPUT    
AS

BEGIN

DECLARE @client_amount numeric(19,4),
	@client_matched numeric(19,4)


SELECT @true = "N"

SELECT @client_amount = 
			(SELECT sum(T2.amount)
 			 	FROM Transdetail T,
			      	Transdetail T2,
			      	Account A
	 
				WHERE   T.transdetail_id = @transdetail_id
				AND     T2.document_id = T.document_id
				AND     A.account_id = T2.account_id
				AND 	A.ledger_id = 2 
 			)
SELECT @client_matched =  
			(SELECT sum(M.base_match_amount)
 			  	FROM Transdetail T,
			      	Transdetail T2,
			      	Account A,
			      	Transmatch M
	 
				WHERE   T.transdetail_id = @transdetail_id
				AND     T2.document_id = T.document_id
				AND     A.account_id = T2.account_id
				AND 	A.ledger_id = 2
				AND 	T2.transdetail_id = M.transdetail_id
				AND NOT EXISTS 
					(Select t3.transdetail_id 
					from transdetail t3,
					account a3
					where t3.document_id = T.document_id
					and t3.account_id = a3.account_id
					and a3.ledger_id = 10)
			)

/* Set transaction as fully matched if there was a DID and no Subagent */

SELECT @client_matched =  @client_amount
			WHERE
			NOT EXISTS 
					(Select t.transdetail_id 
					from transdetail t,
					     transdetail t2,
					     account a
					where t.transdetail_id = @transdetail_id  
					and t.document_id = T.document_id
					and t2.document_id = t.document_id
					and t2.account_id = a.account_id
					and a.ledger_id = 10
					)
			AND EXISTS
					(select d.document_id
					from document d,
					     transdetail t
					where t.transdetail_id = @transdetail_id
					and t.document_id + 1 = d.document_id
					and d.documenttype_id in (33,34)
					)
			
			 


SELECT @true = "Y"
	WHERE @client_amount - @client_matched = 0
		
 
END
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

