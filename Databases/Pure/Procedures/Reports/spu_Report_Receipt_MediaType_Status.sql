SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


DDLDropPROCEDURE 'spu_Report_Receipt_MediaType_Status'
GO

CREATE PROCEDURE spu_Report_Receipt_MediaType_Status   
@branch_id int,           
@Start_date datetime=NULL,          
@End_Date datetime=NULL,          
@Bank varchar(100)=NULL,          
@MediaTypeStatus varchar(100)=NULL,  
@group_by varchar(30)=NULL          
AS          
          
SELECT DISTINCT              
  ACT.Short_code AS AccountCode,              
  ADT1.Document_Ref AS DocumentRef,            
  IFI.Insurance_Ref AS PolicyNumber,              
  CLI.Collection_Date as CollectionDate,              
  MTP.Description AS MediaTypeCode,              
  CLI.Media_Ref AS MediaReference,              
  MTS.Description AS MediaType_Status_Code,              
  CLI.Amount As Amount,          
  RMTH.Date_Modified AS UpdateDate,          
  RMTH.Comments AS Comments,        
  CASE @group_by    
        WHEN 'Branch' THEN SRC.Description    
        WHEN 'Bank' THEN CLB.Description    
        WHEN 'Media Type Status' THEN MTS.Description    
        ELSE ''    
    END 'GroupByCode'         
 FROM              
  CashListItem CLI              
  INNER JOIN CashList CLS              
   ON CLS.CashList_Id=CLI.CashList_Id              
  INNER JOIN CashListType CLT              
   ON CLT.CashListType_Id=CLS.CashListType_id              
   AND CLT.Code='R'            
  INNER JOIN Source SRC              
   ON SRC.Source_Id=CLS.Company_Id              
  INNER JOIN MediaType MTP              
   ON MTP.MediaType_Id=CLI.MediaType_Id              
  LEFT JOIN MediaType_Status MTS              
   ON MTS.MediaType_Status_Id=CLI.MediaType_Status_Id              
  LEFT JOIN CashListItem_Bank CLB              
   ON CLB.CashListItem_Bank_id=CLI.CashListItem_Bank_id              
  INNER JOIN AllocationDetail ADT1              
   ON ADT1.TransDetail_Id=CLI.TransDetail_Id              
  INNER JOIN AllocationDetail ADT              
   ON ADT1.Allocation_Id=ADT.Allocation_Id              
  INNER JOIN DocumentType DTP              
   ON DTP.DocumentType_Id=ADT1.DocumentType_Id              
   AND DTP.Code='SRP'              
  INNER JOIN TransDetail TDL              
   ON TDL.TransDetail_Id=ADT.TransDetail_ID              
  INNER JOIN Document DMT              
   ON DMT.Document_Id=TDL.Document_Id              
  INNER JOIN Insurance_File IFI              
   ON IFI.Insurance_File_Cnt=DMT.Insurance_File_Cnt              
  INNER JOIN Account ACT              
   ON ACT.Account_id=CLI.Account_id          
  LEFT JOIN  Receipt_MediaType_Status_History RMTH          
   ON RMTH.CashListItem_id=CLI.CashListItem_id          
      AND RMTH.MediaType_Status_Id=CLI.MediaType_Status_Id          
 WHERE              
  ((ISNULL(@Branch_Id,0)=0) OR CLS.Company_Id=@Branch_Id)              
  AND (ISNULL(@Start_date,0)=0 OR CLI.Collection_Date>=@Start_date)              
  AND (ISNULL(@End_Date,0)=0 OR CLI.Collection_Date<=@End_Date)              
  AND (ISNULL(@MediaTypeStatus,'ALL')='ALL' OR MTS.Description=@MediaTypeStatus)              
  AND (ISNULL(@Bank,'ALL')='ALL' OR CLB.Description=@Bank) 
GO