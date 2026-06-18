SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_get_source_id_on_process'
GO

Create procedure spu_get_source_id_on_process
	@cashlist_id		INT = NULL,
	@cashlistitem_id	INT = NULL
AS

IF ISNULL(@cashlist_id,0)<> 0  
BEGIN  
	SELECT  company_id 
			From CashList With (NoLock) 
				Where CashList_ID = @cashlist_id  
END  
ELSE IF ISNULL(@cashlistitem_id,0)<> 0  
BEGIN  
	SELECT  company_id 
	From CashListItem With (NoLock) 
	Join CashList With (NoLock) 
	ON CashListItem.CashList_id  = CashList.CashList_id 
	Where CashListItem.cashlistitem_id = @cashlistitem_id  
END  
