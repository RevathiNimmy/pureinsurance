SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_UpdateCashListItem_Payment_Status'  
GO

CREATE PROCEDURE spu_SAM_UpdateCashListItem_Payment_Status  
    @nCashListItem_ID INT ,  
    @nCashListItemPayment_StatusId INT  
AS  
  
BEGIN  
 Update CashListItem set cashlistitem_payment_status_id = @nCashListItemPayment_StatusId where cashlistitem_id = @nCashListItem_ID   
  
END  
    
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
