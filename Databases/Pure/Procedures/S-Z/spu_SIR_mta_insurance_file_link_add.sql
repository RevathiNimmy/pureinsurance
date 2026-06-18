SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_mta_insurance_file_link_add'
GO


CREATE PROCEDURE spu_SIR_mta_insurance_file_link_add  
    @insurance_file_cnt INT ,  
    @sequence_number INT,  
    @type_ind SMALLINT,  
    @processed_ind TINYINT,  
    @original_linked_insurance_file_cnt INT,  
    @cancelled_linked_insurance_file_cnt INT,  
    @new_linked_insurance_file_cnt INT,  
    @original_insurance_file_status_id INT,
    @isDirty BIT        
AS  
      
    INSERT INTO mta_insurance_file_link  
        (  
        insurance_file_cnt,  
 sequence_number,  
 type_ind ,  
 processed_ind,  
 original_linked_insurance_file_cnt ,  
 cancelled_linked_insurance_file_cnt,  
 new_linked_insurance_file_cnt,  
        original_insurance_file_status_id,
        IsDirty  
        )  
    VALUES  
        (  
        @insurance_file_cnt,  
 @sequence_number,  
 @type_ind ,  
 @processed_ind,  
 @original_linked_insurance_file_cnt ,  
 @cancelled_linked_insurance_file_cnt,  
 @new_linked_insurance_file_cnt,  
        @original_insurance_file_status_id,
        @isDirty  
        )  
  
  


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
