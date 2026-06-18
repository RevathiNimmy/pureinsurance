EXEC DDLDropProcedure 'spu_ACT_Get_ExtendedTransactions'
GO
CREATE PROCEDURE spu_ACT_Get_ExtendedTransactions 
	@transdetail_id varchar(5000)
AS

	DECLARE @document_id as int
       
    CREATE TABLE #TMPTRANS
    (short_code CHAR(30),
    amount NUMERIC(19,4) , 
    code CHAR(10), 
    document_ref VARCHAR(25),
	 os_amount NUMERIC(19,4), 
	effective_date DATETIME, 
	currency_amount NUMERIC(19,4),
	outstanding_currency_amount NUMERIC(19,4), 
	transdetailex_id INT, 
	transdetail_id INT,
	Total_Count INT)
    
    INSERT INTO #TMPTRANS
    SELECT  a.short_code, 
    td.currency_amount , 
    c.code, 
    d.document_ref, 
   td.outstanding_currency_amount , 
   tex.effective_date, 
   tex.currency_amount,
   tex.outstanding_currency_amount, 
   tex.transdetailex_id,
   TD.transdetail_id ,0
	 
	From transdetail td
	Inner Join TransDetailEx tex ON tex.transdetail_id = td.transdetail_id
	Inner Join Account a ON a.account_id = td.account_id
	Inner Join Document d ON d.document_id = td.document_id
	Inner Join Currency c ON c.currency_id = td.currency_id
	Where td.transdetail_id in ( SELECT * FROM UF_StringToTable(@transdetail_id))
	order by td.transdetail_id ,tex.transdetailex_id 
	
		
UPDATE #TMPTRANS SET Total_Count = (SELECT COUNT(*) FROM  TransDetailEx  WHERE   transdetail_id = #TMPTRANS.transdetail_id)

SELECT * FROM #TMPTRANS

DROP TABLE #TMPTRANS
	
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO
