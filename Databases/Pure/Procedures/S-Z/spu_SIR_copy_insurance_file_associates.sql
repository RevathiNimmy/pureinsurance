SET QUOTED_IDENTIFIER ON
GO
EXECUTE DDLDropProcedure 'spu_SIR_copy_insurance_file_associates'
GO
CREATE PROCEDURE spu_SIR_copy_insurance_file_associates
      @Old_Insurance_File_Cnt INT,
      @New_Insurance_File_Cnt INT

AS
BEGIN

IF @Old_Insurance_File_Cnt = @New_Insurance_File_Cnt
BEGIN
    RAISERROR('Invalid Insurance key %i,for old Insurance_cnt %d', 12,1,@New_Insurance_File_Cnt,@Old_Insurance_File_Cnt)
     RETURN 0
END
ELSE
BEGIN
INSERT INTO Insurance_File_Associates (
    Party_cnt, Association_type_id, date_attached,
    date_removed, Is_Deleted, Association_detail, Insurance_file_cnt)

Select Party_cnt, Association_type_id,
    (Select  Case   
    When  INFT.Code='RENEWAL' AND  Insurance_File_Associates.Is_Deleted IS NULL
    Then 
        Inf.inception_date_tpi 
    ELSE
        Insurance_File_Associates.date_attached										  											  
    End As AttachedDate From  Insurance_File  INF INNER JOIN Insurance_File_type INFT ON INFT.Insurance_File_type_Id=INF.Insurance_File_type_Id
    Where  Insurance_File_cnt=@New_Insurance_File_Cnt) As date_attached,
    date_removed, Is_Deleted, Association_detail, @New_Insurance_File_Cnt  From Insurance_File_Associates
WHERE Insurance_file_cnt = @Old_Insurance_File_Cnt
END
END 
GO
