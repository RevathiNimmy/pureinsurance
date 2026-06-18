SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

Execute DDLDropProcedure 'Spu_bank_guarantee_sel'
GO

CREATE PROCEDURE spu_bank_guarantee_sel  
 @party_code  VARCHAR(20) = NULL,  
 @agent_code  VARCHAR(20) = NULL,  
 @insurance_ref VARCHAR(30) = NULL,  
 @bank_guarantee_ref VARCHAR(50) = NULL,  
 @bank_name   VARCHAR(50) = NULL,  
 @BG_status_id INT = NULL,  
 @MaxRowsToFetch INT = -1  
  
AS  
BEGIN  
 DECLARE @SQL  VARCHAR(8000)  
 DECLARE @Columns VARCHAR(6000)  
 DECLARE @Where VARCHAR(6000)  
 SET @Where=''  
 
 UPDATE Bank_Guarantee SET Bg_Status_Id = 5 where expiry_date < GetDate()


 SET @Columns =  '-1  RowStatus,  
             0  RowIndex,  
             BG.bg_id,  
              bank_name_id,  
              bank_branch,  
      Party_Cnt,  
      BG_ref,  
              BG_currency_Id,  
              BG_limit,  
              available_bal,  
              expiry_date,  
              is_policy_lock,  
             
              CASE (SELECT COUNT(*) FROM BG_BRANCH_LINK WHERE BG_ID = BG.BG_ID)  
               WHEN 1 THEN  
                   (SELECT TOP 1 SRC.Description  
         FROM SOURCE SRC INNER JOIN BG_BRANCH_LINK BGBL  
             ON BGBL.Source_id = SRC.Source_Id  
         WHERE BGBL.bg_id = BG.bg_id)  
         WHEN 0 THEN  
        NULL  
         ELSE  
        ''MULTIPLE''  
              END AS Branches,
		CASE (SELECT COUNT(*) FROM BG_PRODUCT_LINK WHERE BG_ID = BG.BG_id)  
               WHEN 1 THEN  
                   (SELECT TOP 1 PDCT.Description  
         FROM PRODUCT PDCT INNER JOIN BG_Product_LINK BGPL  
             ON BGPL.Product_id = PDCT.Product_id  
         WHERE BGPL.bg_id = BG.bg_id)  
         WHEN 0 THEN  
        NULL  
         ELSE  
        ''MULTIPLE''  
              END AS Products,    
              is_deleted,  
        (SELECT resolved_name  
       FROM Party PTY  
       WHERE PTY.Party_cnt =BG.Party_cnt) AS ResolvedName,  
      (SELECT shortname  
       FROM Party PTY  
       WHERE PTY.Party_cnt =BG.Party_cnt) AS ShortName,  
      (SELECT description  
       FROM BG_Status BGS  
       WHERE BGS.BG_status_id=BG.BG_status_id)AS Description,  
      (SELECT bank_name  
       FROM Bank BNK  
       WHERE BNK.Bank_Id = BG.Bank_Name_Id)AS BankName'  
 SET @SQL=''   
 IF @MaxRowsToFetch<>-1  
 BEGIN  
 SET @sql = @sql +'SET NOCOUNT ON' + CHAR(13) + CHAR(10)     
 SET @sql = @sql +'SET ROWCOUNT '  
 SET @sql = @sql + CONVERT(VARCHAR(5),@MaxRowsToFetch) + CHAR(13) + CHAR(10)  
 END  
  
SET @SQL = @SQL+ 'SELECT ' + @Columns + ' FROM Bank_Guarantee BG '    
 IF ISNULL(@insurance_ref,'')= ''               AND ISNULL(@party_code,'') = ''  
  AND ISNULL(@agent_code,'')= ''  
  AND ISNULL(@bank_name,'') = ''  
  AND ISNULL(@BG_status_id,0) = 0  
  AND ISNULL(@bank_guarantee_ref,'') =''  
 BEGIN  
  SET @Where='WHERE 1=2'  
 END  
 ELSE  
 BEGIN  
  SET @Where=' WHERE BG.BG_status_id = ' + CONVERT(VARCHAR(10),@BG_status_id)  
  --If @insurance_ref is provided  
  IF ISNULL(@insurance_ref,'')<> ''  
  BEGIN  
   SET @SQL=@SQL + ' INNER JOIN Insurance_File_BG_Link BGIF ON BG.bg_id = BGIF.bg_id '  
   SET @Where= @Where+ ' AND BGIF.insurance_file_cnt in (select insurance_file_cnt from insurance_file where insurance_ref like ''' + @insurance_ref + ''')'  
  END  
  
  --If @party_code is provided  
  IF ISNULL(@party_code,'') <> ''  
  BEGIN  
   SET @Where=@Where+' AND Party_Cnt in (select party_cnt from party where shortname like ''' + @party_code + ''')'  
  END  
  ELSE  
  --If @agent_code is provided  
  IF ISNULL(@agent_code,'') <> ''  
  BEGIN  
   SET @Where=@Where+' AND Party_Cnt in (select party_cnt from party  inner join party_type pt on pt.party_type_id= party.party_type_id  and pt.code=''AG'' where  shortname like ''' + @agent_code + ''')'  
  END  
  
  --If @bank_name_id is provided  
  IF ISNULL(@bank_name,'') <> ''  
  BEGIN  
   SET @Where=@Where+' AND bank_name_id in (select bank_id from Bank where code like ''' + @bank_name + ''')'  
  END  
  
  --If @BG_status_id is provided  
--   IF ISNULL(@BG_status_id,0) <> 0  
--   BEGIN  
--    SET @Where=@Where+' AND BG.BG_status_id = ' + CONVERT(VARCHAR(10),@BG_status_id)  
--   END  
  
  --If @bank_guarantee_ref is provided  
 IF ISNULL(@bank_guarantee_ref,'') <>''  
  BEGIN  
 SET @Where=@Where+' AND bg_ref LIKE ''' + CONVERT(VARCHAR(50),@bank_guarantee_ref) + ''''  
 END  
 END  
  
 SET @SQL = @SQL + @Where  
 EXEC (@SQL)  
 --Print @SQL
IF @MaxRowsToFetch<>-1  
 BEGIN  
 SET @sql = @sql +  CHAR(13) + CHAR(10) + 'SET ROWCOUNT 0'   
 SET @sql = @sql +  CHAR(13) + CHAR(10) + 'SET NOCOUNT OFF'  
 END  
END  


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

