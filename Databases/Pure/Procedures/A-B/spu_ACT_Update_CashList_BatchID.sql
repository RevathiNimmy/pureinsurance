SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Update_CashList_BatchID'
GO


CREATE PROCEDURE spu_ACT_Update_CashList_BatchID
    @nBatchID INT,
    @nCashListID INT
AS


BEGIN
    UPDATE CASHLIST
    SET PMNav_Batch_Key =@nBatchID
    WHERE cashlist_id=@nCashListID 
    
END
GO


