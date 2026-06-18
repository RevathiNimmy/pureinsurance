SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Del_Last_Print_Run'
GO


CREATE PROCEDURE spu_Del_Last_Print_Run
    @renewal_status_type_id int,  
    @compare_Date datetime,  
    @Start_Date datetime = null  
AS  
  
IF @Start_Date Is Null  
 DELETE  FROM Last_Print_Run  
 WHERE   renewal_status_cnt in  
 (  
 SELECT  RS.renewal_status_cnt  
 FROM    Renewal_Status RS,  
  Product Prod,  
  Insurance_File InsFile  
 WHERE   RS.renewal_status_type_id = @renewal_status_type_id  
 AND RS.insurance_file_cnt = InsFile.insurance_file_cnt  
 AND InsFile.product_id = Prod.product_id  
 AND InsFile.expiry_date <= dateadd(week,Prod.renewal_period,@compare_date)  
 )  
ELSE  
 DELETE  FROM Last_Print_Run  
 WHERE   renewal_status_cnt in  
 (  
 SELECT  RS.renewal_status_cnt  
 FROM    Renewal_Status RS,  
  Product Prod,  
  Insurance_File InsFile  
 WHERE   RS.renewal_status_type_id = @renewal_status_type_id  
 AND RS.insurance_file_cnt = InsFile.insurance_file_cnt  
 AND InsFile.product_id = Prod.product_id  
    AND InsFile.renewal_date BETWEEN @Start_Date AND @compare_date)  
    --DC140305 PN19397 changed check on dates as requested by Thinny  
	

GO


