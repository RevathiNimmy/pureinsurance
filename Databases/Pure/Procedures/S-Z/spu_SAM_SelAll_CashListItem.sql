SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_SelAll_CashListItem'
GO

--Start (Abhishek,Muthu Kumari) - (Tech Spec -UIIC WR62 – Cash Cheque Receipt – Get Receipt Cash List Items.doc) - (6)
CREATE PROCEDURE spu_SAM_SelAll_CashListItem  
    @cashlist_id int  
AS  
 
SELECT  
ci.Cashlistitem_id AS CashListItemKey,
ci.Media_ref AS MediaReference,
mt.Description MediaType,
ci.Amount AS Amount,
ac.Short_code AS AccountShortCode,
cps.Description AS Status,
ci.Letter as Letter
 FROM CashListItem ci 
INNER join Mediatype mt
ON ci.mediatype_id=mt.mediatype_id
INNER join account ac
ON ci.account_id=ac.account_id
INNER join Cashlistitem_Payment_status cps
ON ci.cashlistitem_payment_status_id=cps.cashlistitem_payment_status_id
WHERE cashlist_id =@cashlist_id 
GO
--End (Abhishek,Muthu Kumari) - (Tech Spec -UIIC WR62 – Cash Cheque Receipt – Get Receipt Cash List Items.doc) - (6)

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS OFF
GO