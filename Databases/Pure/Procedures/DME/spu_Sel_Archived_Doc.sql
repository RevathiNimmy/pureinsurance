EXECUTE DDLDropProcedure 'spu_Sel_Archived_Doc'
GO
CREATE PROCEDURE spu_Sel_Archived_Doc  
 @dmeDoc Varchar(255) OUTPUT,  
 @sharePointDoc Varchar(255) OUTPUT,  
 @fullPath Varchar(255) OUTPUT,
 @created_by Varchar(255) OUTPUT,  
 @created_date DATETIME OUTPUT,
  
    @doc_Num INT  
AS  
BEGIN  
Declare  
@folderNum Int,  
@parentNum Int  
  
Select @fullPath = ''  
  
Select @dmeDoc = dev.server_unc + dev.share_name + dev.drive + vol.directory + RTRIM(dp.page_name) + '.' + RTRIM(page_type),  
@sharePointDoc = CASE  
     WHEN df.folder_level=0 THEN REPLACE(RTRIM(df.folder_name),'/','') + '/' + REPLACE(RTRIM(doc.doc_name),'/','') + '.' + RTRIM(dp.page_type)  
     WHEN df.folder_level=1 THEN REPLACE(RTRIM(df.folder_name),'/','') + '/' + REPLACE(RTRIM(doc.doc_name),'/','') + '.' + RTRIM(dp.page_type)  
     WHEN df.folder_level>=2 THEN  
      CASE  
       WHEN LOWER(left(df.ex_code,1)) = 'c' THEN  
			'Claim/' +  REPLACE(RTRIM(df.folder_name),'/','') + '/' + REPLACE(RTRIM(doc.doc_name),'/','') + '.' + RTRIM(dp.page_type)  
       WHEN df.ex_code <> '' AND UPPER(df.ex_code) <> 'GENERAL' THEN  
			'Policy/' +  REPLACE(RTRIM(df.folder_name),'/','') + '/' + REPLACE(RTRIM(doc.doc_name),'/','') + '.' + RTRIM(dp.page_type)
       WHEN df.ex_code <> '' AND UPPER(df.ex_code) = 'GENERAL' THEN  
			REPLACE(RTRIM(df.folder_name),'/','') + '/' + REPLACE(RTRIM(doc.doc_name),'/','') + '.' + RTRIM(dp.page_type)  
       ELSE  
			'General/' +  REPLACE(RTRIM(df.folder_name),'/','') + '/' + REPLACE(RTRIM(doc.doc_name),'/','') + '.' + RTRIM(dp.page_type)  
       END  
     ELSE  
		REPLACE(RTRIM(df.folder_name),'/','') + '/' + REPLACE(RTRIM(doc.doc_name),'/','') + '.' + RTRIM(dp.page_type)  
     END  
    ,  
@folderNum = df.parent_num,  
@created_date=ddi.doc_date,
@created_by =ddi.scan_user 

 From DOC_document doc  
  Inner Join DOC_page dp ON dp.doc_num = doc.doc_num  
  Inner Join DOC_volume vol ON vol.volume_id = dp.volume_id  
  Inner Join DOC_device dev ON dev.device_id = vol.device_id  
  Inner Join DOC_folder df ON df.folder_num = doc.folder_num  
  Inner Join DOC_doc_info ddi ON ddi.doc_num = doc.doc_num 
  Where doc.doc_num = @doc_Num  
  
DECLARE @isExists INT  
exec master.dbo.xp_fileexist @dmeDoc, @isExists OUTPUT  
  
If @isExists = 0 -- try removing volume  

BEGIN

 Select @dmeDoc = dev.server_unc + dev.share_name + dev.drive + RTrim(dp.page_name) + '.' + RTrim(page_type),  
 @sharePointDoc = REPLACE(RTrim(df.folder_name),'/','') + '/' + REPLACE(RTRIM(doc.doc_name),'/','') + '.' + RTRIM(dp.page_type),  
 @folderNum = df.parent_num,  
 @created_date=ddi.doc_date,
 @created_by =ddi.scan_user 

  From DOC_document doc  
   Inner Join DOC_page dp ON dp.doc_num = doc.doc_num  
   Inner Join DOC_volume vol ON vol.volume_id = dp.volume_id  
   Inner Join DOC_device dev ON dev.device_id = vol.device_id  
   Inner Join DOC_folder df ON df.folder_num = doc.folder_num  
   Inner Join DOC_doc_info ddi ON ddi.doc_num = doc.doc_num 

  Where doc.doc_num = @doc_Num  

END
  
-- re-check else report back  
exec master.dbo.xp_fileexist @dmeDoc, @isExists OUTPUT  
If @isExists = 0 -- report blank  
BEGIN  
 Select @dmeDoc = ''  
END  
  
While @folderNum <> 0  
Begin  
 Select @parentNum = parent_num,  
  @sharePointDoc = Case folder_level  
   When 0 Then @sharePointDoc Else REPLACE(Rtrim(folder_name),'/','') + '/' +  @sharePointDoc End  
  From DOC_folder Where folder_num = @folderNum  
  
  if @parentNum = 0 -- reached to end; get full path  
   Select @fullPath = REPLACE(Rtrim(folder_name),'/','') + '/' +  @sharePointDoc  
     From DOC_folder Where folder_num = @folderNum;  
  Set @folderNum = @parentNum  
End;  
  
With SPECIAL_CHARACTER as  
 (  
 SELECT '~' as item  
 UNION ALL  
SELECT '#' as item  
 UNION ALL  
SELECT '%' as item  
 UNION ALL  
SELECT '&' as item  
 UNION ALL  
SELECT '*' as item  
 UNION ALL  
SELECT '{' as item  
 UNION ALL  
SELECT '}' as item  
 UNION ALL  
SELECT '\' as item  
 UNION ALL  
SELECT ':' as item  
 UNION ALL  
SELECT '<' as item  
 UNION ALL  
SELECT '>' as item  
 UNION ALL  
SELECT '+' as item  
 UNION ALL  
SELECT '|' as item  
 UNION ALL  
SELECT '"' as item  
 )  
  
SELECT @sharePointDoc = Replace(@sharePointDoc, ITEM, '')  
 FROM SPECIAL_CHARACTER  

 select @created_date
 select @created_by
   
END;   
GO  