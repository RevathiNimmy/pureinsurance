-- This will delete all allocation details and the document itself from the system including stats info

DDLDropProcedure 'spu_DeleteDocument'
Go

CREATE PROCEDURE spu_DeleteDocument @DocRef varchar(25)
AS  
BEGIN  
  
DECLARE @DocID int  
  
 Begin Transaction  
  
 --get document id  
  
 SELECT @DocID = document_id FROM Document WHERE Document_Ref = @DocRef  
  
 -- Delete from TransMatch  
 DELETE  TransMatch  
    FROM    TransMatch tm  
    JOIN    transdetail td ON tm.transdetail_id = td.transdetail_id  
 WHERE td.document_id = @DocID  
 IF @@ERROR <> 0  
  GOTO Err_Routine  
  
 -- Delete from MatchGroup  
 DELETE  MatchGroup  
 FROM MatchGroup mg  
    JOIN    TransMatch tm ON tm.match_id = mg.match_id  
    JOIN    transdetail td ON td.transdetail_id = tm.transdetail_id  
 WHERE td.document_id = @DocID  
 IF @@ERROR <> 0  
  GOTO Err_Routine  
  
 -- Delete from AllocationDetail  
 DELETE AllocationDetail  
    FROM    AllocationDetail ad  
    JOIN    transdetail td ON ad.transdetail_id = td.transdetail_id  
 WHERE td.document_id = @DocID  
 IF @@ERROR <> 0  
  GOTO Err_Routine  
  
 -- Delete from Allocation  
 DELETE  Allocation  
 FROM Allocation a  
    JOIN    AllocationDetail ad ON ad.allocation_id = a.allocation_id  
    JOIN    transdetail td ON td.transdetail_id = ad.transdetail_id  
 WHERE td.document_id = @DocID  
 IF @@ERROR <> 0  
  GOTO Err_Routine  
  
 -- Delete from cashlistitem  
 DELETE CashListitem  
 FROM CashListitem cli  
 JOIN Transdetail td ON td.transdetail_id = cli.transdetail_id  
 WHERE td.document_id = @DocID  
 IF @@ERROR <> 0  
  GOTO Err_Routine  
  
 -- Delete from cashlist  
 DELETE CashList  
 FROM CashList cl JOIN CashListitem cli ON cl.cashlist_id = cli.cashlist_id  
 JOIN Transdetail td ON td.transdetail_id = cli.transdetail_id  
 WHERE td.document_id = @DocID  
 IF @@ERROR <> 0  
  GOTO Err_Routine  
  
 -- Delete Cheque  
 DELETE Cheque  
 FROM Cheque c  
 JOIN Transdetail td ON td.transdetail_id = c.transdetail_id  
 WHERE td.document_id = @DocID  
 IF @@ERROR <> 0  
  GOTO Err_Routine  
  
-- Suspended_Accounts_Transactions
 DELETE Suspended_Accounts_Transactions  
 FROM Suspended_Accounts_Transactions c  
 JOIN Transdetail td ON td.transdetail_id = c.suspended_transdetail_id 
 WHERE td.document_id = @DocID  
 IF @@ERROR <> 0  
  GOTO Err_Routine    

 -- Suspended_Accounts_Transactions
 DELETE Released_Accounts_Transactions  
 FROM Released_Accounts_Transactions r  
 JOIN Transdetail td ON td.transdetail_id = r.suspended_transdetail_id 
 WHERE td.document_id = @DocID  
 IF @@ERROR <> 0  
  GOTO Err_Routine  
 
 -- Delete from Transdetail  
 DELETE Transdetail  
 WHERE document_id = @DocID  
 IF @@ERROR <> 0  
  GOTO Err_Routine  
  
 -- delete transaction_export_detail  
 DELETE Transaction_Export_Detail  
  FROM Transaction_Export_Folder tef JOIN Transaction_Export_Detail ted ON tef.transaction_export_folder_cnt = ted.transaction_export_folder_cnt  
  WHERE tef.document_ref = @DocRef  
 IF @@ERROR <> 0  
  GOTO Err_Routine  
  
 -- delete transaction_export_folder  
 DELETE Transaction_Export_Folder WHERE document_ref = @DocRef  
 IF @@ERROR <> 0  
  GOTO Err_Routine  
  
 -- delete stats_detail  
 DELETE Stats_Detail  
  FROM Stats_Folder sf JOIN Stats_Detail sd ON sf.stats_folder_cnt = sd.stats_folder_cnt  
  WHERE sf.document_ref = @DocRef  
 IF @@ERROR <> 0  
  GOTO Err_Routine  

 -- Delete from Credit_Control_Item  

  DELETE Credit_Control_Item WHERE document_id = @DocID  

IF @@ERROR <> 0  

  GOTO Err_Routine  
  
 -- delete stats_folder  
 DELETE Stats_Folder WHERE document_ref = @DocRef  
 IF @@ERROR <> 0  
  GOTO Err_Routine  
  
 -- Delete from Document  
 DELETE Document  
 FROM Document  
 WHERE document_id = @DocID  
 IF @@ERROR <> 0  
  GOTO Err_Routine  
  
 Commit Transaction  
 Print 'Document deleted : ' + @DocRef  
  
 Return  
  
Err_Routine:  
  
 Rollback Transaction  
 Print 'Delete document Failed : ' + @DocRef  
  
END  

	
