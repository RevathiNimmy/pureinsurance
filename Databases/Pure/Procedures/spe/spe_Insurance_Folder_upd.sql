SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spe_Insurance_Folder_upd'
GO

CREATE PROCEDURE spe_Insurance_Folder_upd
    @insurance_folder_cnt int,
    @insurance_folder_id int,
    @source_id smallint,
    @insurance_holder_cnt int,
    @code varchar(40),
    @description varchar(255),
    @inception_date datetime,
    @arc_archive_folder_id int,
    @quote_insurance_ref varchar(30),
    @next_insurance_ref varchar(30),
    @last_insurance_ref varchar(30),
    @renewal_count int  
AS
BEGIN

DECLARE @insurance_file_type_id int    
    SELECT @insurance_file_type_id = insurance_file_type_id FROM insurance_file where insurance_folder_cnt = @insurance_folder_cnt
    and insurance_file_cnt =    (        
                                SELECT MAX(insurance_file_cnt)        
                                FROM    insurance_file        
                                WHERE insurance_folder_cnt = @insurance_folder_cnt        
                                AND insurance_file_type_id IN (1,2,3,5,9,11)        
                            ) 


    UPDATE Insurance_Folder
            SET
            insurance_folder_id=@insurance_folder_id,
            source_id=@source_id,
            insurance_holder_cnt=@insurance_holder_cnt,
            code= (
            -- TB 8/6/03 Added Distinct as existing customer data returns several entries
                SELECT DISTINCT insurance_ref
                from insurance_file
                where insurance_folder_cnt = @insurance_folder_cnt
                and insurance_file_cnt =    (
                                SELECT MAX(insurance_file_cnt)
                                FROM    insurance_file
                                WHERE insurance_folder_cnt = @insurance_folder_cnt
                                AND insurance_file_type_id IN (1,2,3,5,9,11)    
                            )
            ), --  DC250102 was just @code before,
            description=@description,
            inception_date=@inception_date,
            arc_archive_folder_id=@arc_archive_folder_id,
            quote_insurance_ref=CASE 
            WHEN @insurance_file_type_id = 3 THEN (SELECT top 1 insurance_ref      
                from insurance_file      
                where insurance_folder_cnt = @insurance_folder_cnt      
                and insurance_file_cnt =    (      
                                SELECT MAX(insurance_file_cnt)      
                                FROM    insurance_file      
                                WHERE insurance_folder_cnt = @insurance_folder_cnt      
                                AND insurance_file_type_id IN (1,2,3,5,9,11)      
                            ) )  
            ELSE @quote_insurance_ref 
            END,
            next_insurance_ref=@next_insurance_ref,
            last_insurance_ref=@last_insurance_ref,
            renewal_count=@renewal_count  
        WHERE insurance_folder_cnt = @insurance_folder_cnt
    
END
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
