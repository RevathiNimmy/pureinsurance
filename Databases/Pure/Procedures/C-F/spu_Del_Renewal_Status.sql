SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Del_Renewal_Status'
GO


CREATE PROCEDURE spu_Del_Renewal_Status
    @renewal_status_type_id int,  
    @compare_Date datetime,  
    @Start_Date datetime = Null  
AS  
  
-- Tracy Richards 27/08/03 - now deletes from Last_Print_run in case of FK constraint error  
Delete from Last_Print_run  
  
IF @Start_Date Is Null  
 DELETE  FROM Renewal_Status  
 WHERE   insurance_file_cnt in  
 (  
 SELECT  RS.insurance_file_cnt  
 FROM    Renewal_Status RS,  
  Product Prod,  
  Insurance_File InsFile  
 WHERE   RS.renewal_status_type_id = @renewal_status_type_id  
 AND RS.insurance_file_cnt = InsFile.insurance_file_cnt  
 AND InsFile.product_id = Prod.product_id  
 AND InsFile.expiry_date <= dateadd(day,Prod.renewal_period,@compare_date)  
 )  
ELSE  
 DELETE  FROM Renewal_Status  
 WHERE   insurance_file_cnt in  
 (  
 SELECT  RS.insurance_file_cnt  
 FROM    Renewal_Status RS,  
  Product Prod,  
  Insurance_File InsFile  
 WHERE   RS.renewal_status_type_id = @renewal_status_type_id  
 AND RS.insurance_file_cnt = InsFile.insurance_file_cnt  
 AND InsFile.product_id = Prod.product_id  
 AND InsFile.expiry_date BETWEEN @Start_Date AND @compare_Date  
 )  

GO


