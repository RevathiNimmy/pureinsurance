SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Get_AffectedBackDatedMTAVersions'
GO

CREATE PROCEDURE spu_Get_AffectedBackDatedMTAVersions  
	@base_insurance_file_cnt INT  
AS  
BEGIN  

Create table #temp
(
	insurance_file_cnt int,
	insurance_file_type_id int,
	policy_version int,
	TransactionType varchar(10),
	insurance_file_status int	
)
Insert into #temp
(insurance_file_cnt,insurance_file_type_id,policy_version,insurance_file_status)

 SELECT ifile.insurance_file_cnt,  
        ifile.insurance_file_type_id,  
        ifile.policy_version,
        ifile.insurance_file_status_id  
 FROM   insurance_file ifile  
        JOIN insurance_file_type ift  
          ON ifile.insurance_file_type_id = ift.insurance_file_type_id  
 WHERE  ifile.base_insurance_file_cnt = @base_insurance_file_cnt  
        AND insurance_file_cnt <> @base_insurance_file_cnt  

Update t SET TransactionType ='MTA'
FROM #temp T JOIN mta_insurance_file_link L ON L.New_linked_insurance_file_cnt = T.insurance_file_cnt
Where L.New_linked_insurance_file_cnt is not NULL

Update t SET TransactionType ='MTC'
FROM #temp T JOIN mta_insurance_file_link L ON L.cancelled_linked_insurance_file_cnt = T.insurance_file_cnt
Where L.cancelled_linked_insurance_file_cnt is not NULL

Update t SET TransactionType ='REN'
FROM #temp T Where TransactionType ='MTA' AND insurance_file_status IS NULL AND policy_version > 1 AND insurance_file_type_id IN (2, 3)


Select insurance_file_cnt,insurance_file_type_id,policy_version,TransactionType from #temp
END
 

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
