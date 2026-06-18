SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_DOC_merge_folders'
GO

CREATE PROCEDURE spu_DOC_merge_folders
	@insurance_file_cnt VARCHAR(20),
	@party_cnt VARCHAR(20),
	@company_id VARCHAR(20)
	

AS

DECLARE	@folder_num INT,
	@parent_num INT,	
	@Parent_Folder_Num INT
	
SELECT @folder_num = pol.folder_num
FROM doc_folder pol
JOIN doc_folder cli
ON cli.folder_num = pol.parent_num
JOIN doc_folder com
ON com.folder_num = cli.parent_num
WHERE pol.ex_code = @insurance_file_cnt
AND cli.ex_code = @party_cnt
AND com.ex_code = @company_id

UPDATE d
SET d.folder_num = @folder_num
FROM doc_document d
JOIN doc_folder f
ON f.folder_num = d.folder_num
JOIN doc_folder cli  
ON cli.folder_num = f.parent_num   
WHERE f.folder_level = 2
AND f.ex_code = @insurance_file_cnt
AND cli.ex_code = @party_cnt 
AND cli.folder_level = 1




SELECT @Parent_Folder_Num = pol.Parent_num    
FROM doc_folder pol    
JOIN doc_folder cli    
ON cli.folder_num = pol.parent_num    
JOIN doc_folder com    
ON com.folder_num = cli.parent_num    
WHERE pol.ex_code = @insurance_file_cnt    
AND cli.ex_code = @party_cnt    
AND com.ex_code <> @company_id

DELETE
FROM doc_folder
WHERE folder_num in
(
	SELECT pol.folder_num
	FROM doc_folder pol
	JOIN doc_folder cli
	ON cli.folder_num = pol.parent_num
	JOIN doc_folder com
	ON com.folder_num = cli.parent_num
	WHERE pol.ex_code = @insurance_file_cnt
	AND cli.ex_code = @party_cnt
	AND com.ex_code <> @company_id
)


SELECT @parent_num = parent_num
FROM doc_folder 
WHERE folder_num = @folder_num 

UPDATE fc
SET parent_num = @parent_num 
FROM doc_folder fp
JOIN insurance_folder ifo
ON ifo.insurance_folder_cnt = CONVERT(INT, fp.ex_code)
AND fp.folder_level = 2
AND LEFT(fp.ex_code,1) <> 'C'
JOIN insurance_file ifi
ON ifo.insurance_folder_cnt = ifi.insurance_folder_cnt
JOIN claim c
ON c.policy_id = ifi.insurance_file_cnt
JOIN doc_folder fc
ON CONVERT(INT, RIGHT(fc.ex_code, LEN(fc.ex_code)-1)) = c.claim_id
AND fc.folder_level = 2
AND LEFT(fc.ex_code,1) = 'C'
AND fc.ex_code <> 'COMPLAINTS'
AND ISNUMERIC(SUBSTRING(fc.ex_code,2,1)) = 1
JOIN doc_folder fcp
ON fcp.folder_num = fc.parent_num
WHERE fp.folder_num = @folder_num
AND fcp.ex_code = @party_cnt

  
IF NOT EXISTS(SELECT * FROM Doc_Folder WHERE PARENT_NUM =  @Parent_Folder_Num)  
BEGIN  
 DELETE FROM Doc_Folder WHERE Folder_Num = @Parent_Folder_Num  
END   
  
GO