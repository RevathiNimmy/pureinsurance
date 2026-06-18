SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Update_TransMatch_CashListID'
GO


CREATE PROCEDURE spu_ACT_Update_TransMatch_CashListID
    @sProcessType VARCHAR(10),
    @nCashListItemID INT,
    @nBatchID INT=0
AS


BEGIN
 IF ISNULL(@sProcessType,'')=''
    BEGIN
    CREATE TABLE #tmpTransDetail
    ( transdetailid INT)
    
	INSERT INTO #tmpTransDetail
	SELECT PBKV.key_value 
	FROM  PMNav_Batch_Key_Value  PBKV  
	JOIN PMNav_Batch PB ON PBKV.pmnav_batch_id =PB.pmnav_batch_id   
	WHERE pmnav_batch_set_id=@nBatchID  AND PB.code ='ACTINSPAY' 


    UPDATE TransMatch
    SET cashlistitem_id= @nCashListItemID
    WHERE transdetail_id  IN (SELECT transdetailid from #tmpTransDetail) AND  (cashlistitem_id IS NULL or cashlistitem_id=0)
    
    DROP TABLE #tmpTransDetail
    END  
  ELSE IF @sProcessType ='APPROVE' OR @sProcessType ='DECLINE'
    UPDATE TransMatch
    SET cashlistitem_id=NULL
    WHERE cashlistitem_id= @nCashListItemID
 
END
GO


