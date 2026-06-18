SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_count_policies_in_renewal'
GO


CREATE PROCEDURE spu_count_policies_in_renewal
 	@product_id int,
	@transfer_date datetime
AS

CREATE TABLE #MySuperTempTable(  
                     insurance_file_cnt INT,  
                     insurance_ref varchar(50),  
                     shortname varchar(100),  
                     resolved_name varchar(200),  
                     effective_date date,
					 Insurance_File_Type_Id INT)  
INSERT INTO #MySuperTempTable(insurance_file_cnt, insurance_ref, shortname, resolved_name, effective_date, Insurance_File_Type_Id)  
EXEC spu_RI2007Disabled_Portfolio_Policy_Sel @product_id=@product_id,@transfer_date=@transfer_date  
        
SELECT  ISNULL(COUNT(distinct if1.insurance_folder_cnt), 0)  
FROM    renewal_status rs,  
    insurance_file if1,  
    insurance_file if2  
WHERE   if2.insurance_folder_cnt = if1.insurance_folder_cnt  
AND rs.insurance_file_cnt = if2.insurance_file_cnt  
AND if1.insurance_file_cnt in (SELECT insurance_file_cnt 
                   FROM #MySuperTempTable)
                   
DROP TABLE #MySuperTempTable 
