SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_Get_CashList'
GO

CREATE PROCEDURE spu_SAM_Get_CashList  
    @ncashlistitem_id	int = null
 
AS  
 
  
SELECT CL.cashlist_id,  
       bankaccount_id,  
       cashlisttype_id,  
       cashliststatus_id,  
       cashlist_ref,  
       company_id,  
       currency_id,  
       list_date,  
       control_total,  
       item_count  
FROM   CashList CL
INNER JOIN CashListItem CLI
ON CL.Cashlist_id = CLI.CashList_id
WHERE CLI.cashlistitem_id = @ncashlistitem_id


SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS OFF
GO
