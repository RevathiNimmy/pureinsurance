SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GetPaymentMethod'
GO


CREATE  Procedure spu_GetPaymentMethod   
    @insurancefilecnt BIGINT  
AS 

    SELECT Payment_method FROM insurance_file 
    WHERE insurance_file_cnt=@insurancefilecnt  
  
