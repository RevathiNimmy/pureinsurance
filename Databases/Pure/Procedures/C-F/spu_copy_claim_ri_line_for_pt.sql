EXECUTE DDLDropProcedure 'spu_copy_claim_ri_line_for_pt'
GO

CREATE PROCEDURE spu_copy_claim_ri_line_for_pt
@claim_id INT,
@version_id INT,
@old_claim_id INT,
@ribandID INT,
@isCreated INT
AS
DECLARE @ri_arrangement_id INT
SELECT @ri_arrangement_id=ri_arrangement_id from Claim_RI_Arrangement where claim_id =@claim_id and ri_band_id =@ribandID
If @isCreated = 1
BEGIN
 INSERT INTO Claim_ri_Arrangement_Line  
     (Claim_Id,  
      ri_Arrangement_Line_Id,  
      ri_Arrangement_Id,  
      TYPE,  
      Treaty_Id,  
      Party_cnt,  
      xol_Arrangement_Id,  
      Default_Share_Percent,  
      This_Share_Percent,  
      Agreement_Code,  
      Priority,  
      Number_Of_Lines,  
      Line_Limit,  
     Sum_Insured,  
      Reserve,  
      Payment,  
      Salvage,  
      Recovery,  
      This_Reserve,  
      This_Payment,  
      This_Salvage,  
      This_Recovery,  
      Base_Claim_ri_Arrangement_Line_Id,  
      Version_Id,  
      Original_ri_Arrangement_Line_Id,  
                        lower_limit,  
                        Retained,  
   participation_percent,  
                        Grouping,is_obligatory , ri_model_line_id,
              reserve_to_date ,
              payment_to_date ,
              salvage_to_date ,
              recovery_to_date ,
              claim_incurred_to_date ,
              is_pt_archive   )  
  SELECT @claim_id,  
    Claim_ri_Arrangement_Line.ri_arrangement_line_id ,  
    @ri_arrangement_id,  
    Claim_ri_Arrangement_Line.TYPE,  
    Claim_ri_Arrangement_Line.Treaty_Id,  
    Claim_ri_Arrangement_Line.Party_cnt,  
    Copy_Claim_xol_Arrangement.xol_Arrangement_Id,  
     0 ,  
    0,  
    Claim_ri_Arrangement_Line.Agreement_Code,  
    Claim_ri_Arrangement_Line.Priority,  
    Claim_ri_Arrangement_Line.Number_Of_Lines,  
    Claim_ri_Arrangement_Line.Line_Limit,  
    0,  
    0,  
    0,  
    0,  
    0,  
    0,  
    0,  
    0,  
    0, -- Only zero the new 'this' amounts  
    Claim_ri_Arrangement_Line.Base_Claim_ri_Arrangement_Line_Id,  
    @version_id,  
    Original_ri_Arrangement_Line_Id,  
    Claim_ri_Arrangement_Line.lower_limit,  
                Claim_ri_Arrangement_Line.Retained,  
                Claim_ri_Arrangement_Line.participation_percent,  
                Claim_ri_Arrangement_Line.Grouping,Claim_ri_Arrangement_Line.is_obligatory ,  
                Claim_ri_Arrangement_Line.ri_model_line_id,
                Claim_ri_Arrangement_Line.reserve_to_date ,
                Claim_ri_Arrangement_Line.payment_to_date ,
                Claim_ri_Arrangement_Line.salvage_to_date ,
                Claim_ri_Arrangement_Line.recovery_to_date ,
                Claim_ri_Arrangement_Line.payment + Claim_ri_Arrangement_Line.salvage + Claim_ri_Arrangement_Line.recovery  ,
                1  
  FROM   Claim_ri_Arrangement_Line  
    LEFT JOIN Claim_ri_Arrangement  
     ON Claim_ri_Arrangement_Line.ri_Arrangement_Id = Claim_ri_Arrangement.ri_Arrangement_Id  
    LEFT JOIN (SELECT ri_Arrangement_Id,  
         Version_Id,  
         Base_Claim_ri_Arrangement_Id  
        FROM   Claim_ri_Arrangement  
        WHERE  Version_Id = @version_id  
        AND Claim_Id = @claim_id) Copy_Claim_ri_Arrangement  
     ON Copy_Claim_ri_Arrangement.Base_Claim_ri_Arrangement_Id = Claim_ri_Arrangement.Base_Claim_ri_Arrangement_Id  
    LEFT JOIN Claim_xol_Arrangement  
     ON Claim_ri_Arrangement_Line.xol_Arrangement_Id = Claim_xol_Arrangement.xol_Arrangement_Id  
    LEFT JOIN (SELECT xol_Arrangement_Id,  
         Version_Id,  
         Base_Claim_xol_Arrangement_Id  
        FROM Claim_xol_Arrangement  
        WHERE  Version_Id = @version_id  
        AND Claim_Id = @claim_id) Copy_Claim_xol_Arrangement  
     ON Copy_Claim_xol_Arrangement.Base_Claim_xol_Arrangement_Id = Claim_xol_Arrangement.Base_Claim_xol_Arrangement_Id  
  WHERE  Claim_ri_Arrangement_Line.Claim_Id = @old_claim_id  AND type in ('F','TFS','T') and ri_band_id=@ribandID  
  END
  ELSE
  BEGIN
	 INSERT INTO Claim_ri_Arrangement_Line  
     (Claim_Id,  
      ri_Arrangement_Line_Id,  
      ri_Arrangement_Id,  
      TYPE,  
      Treaty_Id,  
      Party_cnt,  
      xol_Arrangement_Id,  
      Default_Share_Percent,  
 This_Share_Percent,  
      Agreement_Code,  
      Priority,  
      Number_Of_Lines,  
      Line_Limit,  
     Sum_Insured,  
      Reserve,  
      Payment,  
      Salvage,  
      Recovery,  
      This_Reserve,  
      This_Payment,  
      This_Salvage,  
      This_Recovery,  
      Base_Claim_ri_Arrangement_Line_Id,  
      Version_Id,  
      Original_ri_Arrangement_Line_Id,  
                        lower_limit,  
                        Retained,  
   participation_percent,  
                        Grouping,is_obligatory , ri_model_line_id,
              reserve_to_date ,
              payment_to_date ,
              salvage_to_date ,
              recovery_to_date ,
              claim_incurred_to_date ,
              is_pt_archive   )  
  SELECT @claim_id,  
    Claim_ri_Arrangement_Line.ri_arrangement_line_id ,  
    @ri_arrangement_id,  
    Claim_ri_Arrangement_Line.TYPE,  
    Claim_ri_Arrangement_Line.Treaty_Id,  
    Claim_ri_Arrangement_Line.Party_cnt,  
    Copy_Claim_xol_Arrangement.xol_Arrangement_Id,  
     0 ,  
   0,  
    Claim_ri_Arrangement_Line.Agreement_Code,  
    Claim_ri_Arrangement_Line.Priority,  
    Claim_ri_Arrangement_Line.Number_Of_Lines,  
    Claim_ri_Arrangement_Line.Line_Limit,  
    0,  
    0,  
    0,  
    0,  
    0,  
    0,  
    0,  
    0,  
    0, -- Only zero the new 'this' amounts  
    Claim_ri_Arrangement_Line.Base_Claim_ri_Arrangement_Line_Id,  
    @version_id,  
    Original_ri_Arrangement_Line_Id,  
    Claim_ri_Arrangement_Line.lower_limit,  
                Claim_ri_Arrangement_Line.Retained,  
                Claim_ri_Arrangement_Line.participation_percent,  
                Claim_ri_Arrangement_Line.Grouping,Claim_ri_Arrangement_Line.is_obligatory ,  
                Claim_ri_Arrangement_Line.ri_model_line_id,
                Claim_ri_Arrangement_Line.reserve_to_date ,
                Claim_ri_Arrangement_Line.payment_to_date ,
                Claim_ri_Arrangement_Line.salvage_to_date ,
                Claim_ri_Arrangement_Line.recovery_to_date ,
                Claim_ri_Arrangement_Line.claim_incurred_to_date  ,
                1  
  FROM   Claim_ri_Arrangement_Line  
    LEFT JOIN Claim_ri_Arrangement  
     ON Claim_ri_Arrangement_Line.ri_Arrangement_Id = Claim_ri_Arrangement.ri_Arrangement_Id  
    LEFT JOIN (SELECT ri_Arrangement_Id,  
         Version_Id,  
         Base_Claim_ri_Arrangement_Id  
        FROM   Claim_ri_Arrangement  
        WHERE  Version_Id = @version_id  
        AND Claim_Id = @claim_id) Copy_Claim_ri_Arrangement  
     ON Copy_Claim_ri_Arrangement.Base_Claim_ri_Arrangement_Id = Claim_ri_Arrangement.Base_Claim_ri_Arrangement_Id  
    LEFT JOIN Claim_xol_Arrangement  
     ON Claim_ri_Arrangement_Line.xol_Arrangement_Id = Claim_xol_Arrangement.xol_Arrangement_Id  
    LEFT JOIN (SELECT xol_Arrangement_Id,  
         Version_Id,  
         Base_Claim_xol_Arrangement_Id  
        FROM Claim_xol_Arrangement  
        WHERE  Version_Id = @version_id  
        AND Claim_Id = @claim_id) Copy_Claim_xol_Arrangement  
     ON Copy_Claim_xol_Arrangement.Base_Claim_xol_Arrangement_Id = Claim_xol_Arrangement.Base_Claim_xol_Arrangement_Id  
  WHERE  Claim_ri_Arrangement_Line.Claim_Id = @old_claim_id  AND type in ('F','TFS','T') AND ri_band_id=@ribandID  AND Claim_ri_Arrangement_Line.is_pt_archive = 1
  END
  GO
