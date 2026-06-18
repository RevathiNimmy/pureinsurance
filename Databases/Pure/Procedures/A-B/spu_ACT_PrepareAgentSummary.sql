SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO
EXECUTE DDLDropProcedure 'spu_ACT_PrepareAgentSummary'
GO

CREATE Procedure spu_ACT_PrepareAgentSummary    
(    
 @transaction_date_from DATETIME,    
 @transaction_date_to DATETIME,    
 @currency_id SMALLINT,    
 @product_id INT,    
 @company_id INT,    
 @user_id INT,    
 @only_within_authority_limit TINYINT,    
 @Agents VARCHAR(MAX),    
 @session_guid VARCHAR(40) OUTPUT    
)    
AS    
    
Declare @SQL VARCHAR(MAX)    
Declare @SQLWhere VARCHAR(MAX)    
Declare @AgentsAccountId INT  
DECLARE @kClientAccountTypeId SMALLINT 
SET @kClientAccountTypeId = 3  

EXEC spu_ACT_DeleteUserSessions_TransDetail_Selection @user_id    
    
SET @session_guid = NEWID()    
    
DECLARE @upper_limit MONEY    
DECLARE @limit_currency_id SMALLINT    
    
If @only_within_authority_limit = 1    
BEGIN    
 SELECT @upper_limit = payments_amount,    
 @limit_currency_id=payments_currency_id    
 FROM User_Authorities    
 WHERE user_id=@user_id    
 AND has_payments_authority=1    
END    
    
IF YEAR(@transaction_date_to)=1899 SET @transaction_date_to = NULL    
    
CREATE TABLE #Temp    
( transdetail_id INT,    
 insurance_file_cnt INT,    
 outstanding_amount MONEY,    
 spare VARCHAR(50),    
 Document_id INT,    
 is_gross_agent INT,
 binder_indicator INT    
)    
    
Declare @Query nvarchar(MAX)    
    
Select @Query = 'INSERT INTO #Temp ( transdetail_id,insurance_file_cnt,outstanding_amount,spare,Document_id,is_gross_agent,binder_indicator)    
  SELECT TD.TransDetail_id,D.Insurance_file_cnt, outstanding_account_amount,spare,D.Document_id,PA.is_gross_agent,ISNULL(PA.binder_indicator,0) binder_indicator    
  FROM Transdetail TD JOIN Document D ON TD.Document_id = D.Document_id    
  LEFT JOIN Transdetail_Type TT ON TD.transdetail_type_id = TT.transdetail_type_id    
  INNER JOIN Account A On TD.account_id  = A.account_id    
  INNER JOIN Party_Agent PA ON A.ACCOUNT_KEY = PA.PARTY_CNT    
  WHERE PA.PARTY_CNT IN ' + @Agents + 'AND TD.SPARE = ''COMM'''    
  
EXEC sp_executesql @Query    
/*    
SELECT @Query = 'INSERT INTO #Temp ( transdetail_id,insurance_file_cnt,outstanding_amount,spare,d.Document_id)    
SELECT TD.TransDetail_id,D.Insurance_file_cnt, outstanding_account_amount,spare,D.Document_id    
FROM Transdetail TD JOIN Document D ON TD.Document_id = D.Document_id    
LEFT JOIN Transdetail_Type TT ON TD.transdetail_type_id = TT.transdetail_type_id    
INNER JOIN Account A ON TD.account_id  = A.account_id    
JOIN Insurance_file IFL ON D.Insurance_file_cnt = IFL.Insurance_file_cnt    
INNER JOIN Party P ON A.Account_key = P.Party_cnt    
Where P.Party_cnt IN(    
 SELECT Insured_cnt FROM insurance_file WHERE Insurance_file_cnt IN    
 (SELECT DISTINCT Insurance_file_cnt FROM #Temp) AND Insured_cnt NOT IN  ' + @Agents +    
') AND TD.Document_id IN( SELECT Document_id FROM #Temp)'    
*/    
--EXEC sp_executesql @Query    
    
--DELETE FROM #Temp WHERE insurance_file_cnt IN    
--( SELECT insurance_file_cnt FROM #Temp WHERE outstanding_amount <>0 AND (spare ='GROSS' OR spare= 'TAX')    
--AND is_gross_agent = 1 )    
    
    
SET @SQL = ' INSERT INTO TRANSDETAIL_SELECTION (transdetail_id, session_guid, user_id, transdetail_selection_type_id ) '    
SET @SQL = @SQL + 'SELECT T.transdetail_id, ' + '''' + @session_guid + '''' + ', ' + Cast(@user_id  AS VarChar) + ', 1 '    
SET @SQL = @SQL + ' FROM #Temp T JOIN Transdetail TD ON T.Transdetail_ID=TD.Transdetail_ID'    
SET @SQL = @SQL + ' LEFT JOIN Insurance_file IFC ON T.Insurance_file_cnt = IFC.Insurance_file_cnt '    
SET @SQLWhere= ' T.outstanding_amount <> 0 AND T.SPARE = ''COMM'' AND (T.binder_indicator=0 OR T.insurance_file_cnt IN (SELECT d.insurance_file_cnt FROM 
  transdetail TRD INNER JOIN document d on trd.document_id=d.document_id
  JOIN account ACC ON ACC.account_id=TRD.account_id
  JOIN AccountType ACCT ON ACC.accounttype_id=ACCT.accounttype_id
  WHERE d.insurance_file_cnt =T.insurance_file_cnt and ACCT.accounttype_id='+  Cast(@kClientAccountTypeId  AS VarChar)  +' AND outstanding_account_amount=0 )) '
    
IF @transaction_date_from <> ''    
BEGIN    
 If @SQLWhere <> ''    
 BEGIN    
  SET @SQLWhere= @SQLWhere + ' AND TD.accounting_date >=' + '''' + CAST(@transaction_date_from AS VARCHAR) + ''''    
 END    
 Else    
 BEGIN    
  SET @SQLWhere= ' TD.accounting_date >=' + '''' + CAST(@transaction_date_from AS VARCHAR) + ''''    
 END    
END    
    
IF ISNULL(@transaction_date_to,'') <> ''    
 BEGIN    
  If @SQLWhere <> ''    
  BEGIN    
   SET @SQLWhere= @SQLWhere + ' AND TD.accounting_date <=' + '''' + CAST(@transaction_date_to AS VARCHAR) + ''''    
  END    
  Else    
  BEGIN    
   SET @SQLWhere= ' TD.accounting_date <=' + '''' + CAST(@transaction_date_to AS VARCHAR) + ''''    
  END    
END    
    
IF @currency_id <> ''    
 BEGIN    
 If @SQLWhere <> ''    
 BEGIN    
  SET @SQLWhere= @SQLWhere + ' AND TD.Account_Currency_ID =' + CAST(@currency_id AS VARCHAR)    
 END    
 Else    
 BEGIN    
  SET @SQLWhere= ' TD.Account_Currency_ID <=' + CAST(@currency_id AS VARCHAR)    
 END    
END    
    
IF @product_id <> ''    
BEGIN    
 If @SQLWhere <> ''    
 BEGIN    
  SET @SQLWhere= @SQLWhere + ' AND IFC.product_id =' + CAST(@product_id AS VARCHAR)    
 END    
 Else    
 BEGIN    
  SET @SQLWhere= ' IFC.product_id <=' + CAST(@product_id AS VARCHAR)    
 END    
END    
    
IF @company_id <> ''    
BEGIN    
 If @SQLWhere <> ''    
 BEGIN    
  SET @SQLWhere= @SQLWhere + ' AND TD.sub_branch_id =' + CAST(@company_id AS VARCHAR)    
 END    
 Else    
 BEGIN    
  SET @SQLWhere= ' TD.sub_branch_id <=' + CAST(@company_id AS VARCHAR)    
 END    
END    
    
IF @user_id <> ''    
BEGIN    
 If @SQLWhere <> ''    
 BEGIN    
  SET @SQLWhere= @SQLWhere + ' AND TD.transdetail_id NOT IN (SELECT transdetail_id FROM TransDetail_Selection WHERE user_id <> ' + CAST(@user_id AS VARCHAR) + ')'    
 END    
 Else    
 BEGIN    
  SET @SQLWhere= ' TD.transdetail_id NOT IN (SELECT transdetail_id FROM TransDetail_Selection WHERE user_id <> ' + CAST(@user_id AS VARCHAR)  + ')'    
 END    
END    
    
If @SQLWhere <> ''    
BEGIN    
 SET @SQLWhere= ' where ' + @SQLWhere    
END    
    
SET @SQL = @SQL + @SQLWhere    
    
EXEC (@SQL)    
    
DROP TABLE #Temp    
    
SELECT    
TD.account_id,    
P.shortname,    
P.resolved_name,    
SUM(TD.outstanding_account_amount) AS Amount,    
C.iso_code,    
TD.account_currency_id    
FROM TransDetail TD    
INNER JOIN TransDetail_Selection TDS ON TD.TransDetail_ID = TDS.TransDetail_ID    
INNER JOIN ACCOUNT A ON TD.account_id = A.account_id    
INNER JOIN Party P on P.party_cnt = A.account_key    
INNER JOIN CURRENCY C on TD.account_currency_id = C.currency_id    
WHERE TDS.session_guid = @session_guid    
GROUP BY    
TD.account_id,    
P.shortname,    
P.resolved_name,    
C.iso_code,    
TD.account_currency_id 
GO
