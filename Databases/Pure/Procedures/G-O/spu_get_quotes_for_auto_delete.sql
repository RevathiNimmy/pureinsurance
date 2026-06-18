SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_get_quotes_for_auto_delete'
GO
CREATE PROCEDURE spu_get_quotes_for_auto_delete    
 @current_date date    
AS    
BEGIN    
    
 SELECT IFL.insurance_file_cnt, IFL.source_id FROM Insurance_File IFL
 INNER JOIN Product P ON P.product_id = IFL.product_id
 INNER JOIN Insurance_File_System IFS ON IFS.insurance_file_cnt = IFL.insurance_file_cnt
 WHERE DATEADD(DAY,(ISNULL(P.delete_quote_after,0) + ISNULL(P.grace_period,0)),
     (Case when IFL.quote_status_id IN(3,4) Then ISNULL(IFL.date_issued, ISNULL(IFS.last_modified, IFS.date_created))
      Else ISNULL(IFS.last_modified, IFS.date_created)
      End)) < @current_date
 AND IFL.quote_base_insurance_file_cnt IS NOT NULL
 AND IFL.insurance_file_type_id IN (1, 4, 7, 10, 12)
 AND EXISTS (
     SELECT 1 FROM Insurance_File IFL2
     WHERE IFL2.quote_base_insurance_file_cnt = IFL.quote_base_insurance_file_cnt
     AND IFL2.insurance_file_type_id = 2
 )    
    
END 	