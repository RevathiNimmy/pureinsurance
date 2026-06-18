SET QUOTED_IDENTIFIER OFF 
GO

SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_get_bank_cheque_sequence'
GO

CREATE PROCEDURE spu_get_bank_cheque_sequence
	@bankaccount_id int
As  
DECLARE @account_id int  
DECLARE @start_cheque_number bigint  

CREATE TABLE #tmpfile
(bankaccount_id INT,
media_ref INT
)

CREATE NONCLUSTERED INDEX [index_1]
ON #tmpfile ([bankaccount_id])
INCLUDE ([media_ref])

BEGIN  
 SELECT @account_id=account_id,
 @start_cheque_number=ISNULL(start_cheque_number,0)
 FROM bankaccount
 WHERE bankaccount_id=@bankaccount_id
 
 INSERT INTO #tmpfile
   SELECT bankaccount_id,CONVERT(numeric,media_ref) FROM  cheque
   WHERE  bankaccount_id =@account_id AND printed_date IS NOT NULL
   AND ISNULL(media_ref,'')<>''
--PN69820  
 SELECT LastSeqNumber
  , NextSeqNumber
  , FirstAvailable = LastSeqNumber + 1
  , LastAvailable = NextSeqNumber - 1
  , NumbersAvailable = NextSeqNumber - (LastSeqNumber + 1)
 FROM (
	  SELECT LastSeqNumber = (SELECT MAX(Seq2.media_ref) AS media_ref
	  FROM  #tmpfile Seq2
	  WHERE Seq2.media_ref < Seq1.media_ref 
	  )
	  , NextSeqNumber = media_ref
	  FROM  #tmpfile Seq1 WHERE media_ref> 0 
	  ) AS A
	  WHERE NextSeqNumber - LastSeqNumber > 1 and LastSeqNumber + 1 >@start_cheque_number
	  ORDER BY LastSeqNumber
END  	
GO


