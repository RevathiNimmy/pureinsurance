SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_sir_check_DME_folder'
GO

CREATE PROCEDURE spu_sir_check_DME_folder
    @sPolicyfolderCnt Varchar(20),
    @sPartyCnt Varchar(20),
    @sSourceId Varchar(20),
    @sFolderDesc Varchar(200)
AS  
BEGIN  

	SELECT   pol.folder_num
		FROM doc_folder pol  
		INNER JOIN doc_folder cli  
			ON cli.folder_num = pol.parent_num  
		INNER JOIN doc_folder com  
			ON com.folder_num = cli.parent_num  
		WHERE pol.ex_code Like @sPolicyfolderCnt  
			AND cli.ex_code Like @sPartyCnt 
			AND com.ex_code Like @sSourceId  
			AND RTrim(pol.folder_name) Like RTrim(@sFolderDesc)
END

GO