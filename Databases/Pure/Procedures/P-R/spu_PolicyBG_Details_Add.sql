SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_PolicyBG_Details_Add'
GO
CREATE PROCEDURE spu_PolicyBG_Details_Add
 @bg_id    INT,
 @insurance_file_cnt  INT,
 @Amount    Numeric(20,2),
 @Cover_From_Date  DATETIME
AS

declare @Due_Date   DATETIME
declare @Available_Balance  Numeric(20,2)

SELECT  @Due_Date=DATEADD(mm, DATEDIFF(mm, '1/1/1900', @cover_from_date )+2, '1/1/1900') - 1

INSERT INTO Insurance_File_BG_Link
   (BG_Id,
   Insurance_File_Cnt,
   Amount,
   DueDate,
   BG_Status_Id,
   BG_Status_Date)
  VALUES (@bg_id,
   @insurance_file_cnt,
   @Amount,
   @Due_Date,
   2,
   GetDate())

UPDATE Insurance_File
       SET
       Balance_Type = 'BG',
       Payment_Method = 'BankGuarantee'
WHERE  Insurance_File_Cnt = @insurance_file_cnt


UPDATE  Bank_Guarantee
        SET available_bal = available_bal - @Amount,
        BG_Status_Id = 2
WHERE   Bg_id=@bg_id

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
