SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXEC DDLDropProcedure 'spu_ACT_Find_Receipt'
GO

CREATE  PROCEDURE spu_ACT_Find_Receipt 
 @Branch_Id INT=NULL,  
 @BankAccount_Id INT = NULL,  
 @ShortName CHAR(20)=NULL,  
 @Party_Cnt INT=NULL,  
 @Insurance_Ref VARCHAR(30)=NULL,  
 @CollectionDate_From DATETIME=NULL,  
 @CollectionDate_To DATETIME=NULL,  
 @MediaReference VARCHAR(30)=NULL,  
 @MediaTypeStatus_Id INT=NULL,  
 @DrawnBank_Id INT=NULL,  
 @Document_Ref VARCHAR(25)=NULL,  
 @MaxRowsToFetch INT = -1,  
 @User_Id SMALLINT,
 @AgentKey INT=0
 
AS  
BEGIN  
  
 IF @MaxRowsToFetch<>-1  
 BEGIN  
 SET NOCOUNT ON  
 SET ROWCOUNT @MaxRowsToFetch  
 END  
  SELECT DISTINCT  
  CLI.cashlistitem_id,  
  IFI.Insurance_File_Cnt,  
  MTP.MediaType_Id,  
  MTP.Code AS MediaTypeCode,  
  MTS.MediaType_Status_Id,  
  MTS.Code AS MediaType_Status_Code,  
  DMT.document_ref document_ref,  
  SRC.Description AS Branch,  
  PTY.ShortName AS ClientCode,  
  LTRIM(RTRIM(PTY.Resolved_Name)) AS ClientName,  
  IFI.Insurance_Ref AS PolicyNumber,  
  MTP.Description AS MediaType,  
  CLI.Media_Ref AS MediaReference,  
  CLB.Description AS DrawnBankName,  
  MTS.Description AS MediaTypeStatus,  
  INFS.CODE AS CurrentStatus  
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
  LEFT JOIN Transdetail TDL  
   ON TDL.Transdetail_id=CLI.Transdetail_id  
  LEFT JOIN Insurance_File_Payment_Details IFPD  
   ON TDL.Transdetail_id=IFPD.Transdetail_id  
  LEFT JOIN Document DMT  
   ON DMT.Document_Id=TDL.Document_Id  
  INNER JOIN Account ACT  
   ON ACT.Account_ID = TDL.Account_ID  
  INNER JOIN Party PTY  
   ON PTY.Party_Cnt=ACT.Account_Key  
  LEFT  JOIN Insurance_File IFI  
   ON IFI.Insurance_File_Cnt=IFPD.Insurance_File_Cnt  
  LEFT JOIN Insurance_File_Status INFS  
   ON IFI.Insurance_File_Status_Id=INFS.Insurance_File_Status_Id  
 WHERE  
  ((ISNULL(@Branch_Id,0)=0  
   AND CLS.Company_Id NOT IN (  
         SELECT  
          source_id  
         FROM  
          PMUser_Source  
         WHERE user_id=@user_id  
          )  
   )OR CLS.Company_Id=@Branch_Id)  
  AND (ISNULL(@BankAccount_Id,0)=0 OR CLS.BankAccount_Id=@BankAccount_Id)  
  AND (ISNULL(@ShortName,'')='' OR PTY.ShortName LIKE LTRIM(RTRIM(@ShortName)))  
  AND (ISNULL(@Insurance_Ref,'')='' OR IFI.Insurance_Ref LIKE LTRIM(RTRIM(@Insurance_Ref)))  
  AND (ISNULL(@CollectionDate_From,0)=0 OR CLI.Collection_Date>=@CollectionDate_From)  
  AND (ISNULL(@CollectionDate_To,0)=0 OR CLI.Collection_Date<=@CollectionDate_To)  
  AND (ISNULL(@MediaReference,'')='' OR CLI.Media_Ref LIKE LTRIM(RTRIM(@MediaReference)))  
  AND (ISNULL(@MediaTypeStatus_Id,0)=0 OR CLI.MediaType_Status_Id=@MediaTypeStatus_Id)  
  AND (ISNULL(@DrawnBank_Id,0)=0 OR CLI.CashListItem_Bank_id=@DrawnBank_Id)  
  AND (ISNULL(@Document_Ref,'')='' OR DMT.Document_Ref LIKE LTRIM(RTRIM(@Document_Ref)))  
  AND (ISNULL(@Party_Cnt,0)=0 OR PTY.Party_Cnt=@Party_Cnt)  
  AND (PTY.agent_cnt=@AgentKey OR @AgentKey=0 OR PTY.party_cnt = @AgentKey) 
  
 IF @MaxRowsToFetch<>-1  
 BEGIN  
 SET ROWCOUNT 0  
 SET NOCOUNT OFF  
 END  
END  
 
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

