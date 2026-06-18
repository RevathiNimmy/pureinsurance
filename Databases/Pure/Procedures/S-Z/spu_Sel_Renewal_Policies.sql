SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Sel_Renewal_Policies'
GO


CREATE PROCEDURE spu_Sel_Renewal_Policies
    @renewal_status_type_id int,  
    @compare_Date datetime,  
    @Start_Date datetime = null  
AS  
  
IF @Start_Date Is Null  
 SELECT  insurance_file_cnt FROM Insurance_File  
 WHERE   insurance_file_cnt in  
 (  
 SELECT  renewal_insurance_file_cnt  
 FROM    Renewal_Status RS,  
  Product Prod,  
  Insurance_File InsFile  
 WHERE   RS.renewal_status_type_id = @renewal_status_type_id  
 AND RS.insurance_file_cnt = InsFile.insurance_file_cnt  
 AND InsFile.product_id = Prod.product_id  
 AND InsFile.expiry_date <= dateadd(day,Prod.renewal_period,@compare_date)  
 )  
ELSE  
 SELECT  insurance_file_cnt FROM Insurance_File  
 WHERE   insurance_file_cnt in  
 (  
 SELECT  renewal_insurance_file_cnt  
 FROM    Renewal_Status RS,  
  Product Prod,  
  Insurance_File InsFile  
 WHERE   RS.renewal_status_type_id = @renewal_status_type_id  
 AND RS.insurance_file_cnt = InsFile.insurance_file_cnt  
 AND InsFile.product_id = Prod.product_id  
 AND InsFile.expiry_date BETWEEN @Start_Date AND @compare_Date  
 )  

GO


