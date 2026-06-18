SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_insert_extra'
GO


CREATE PROCEDURE spu_insert_extra
    @Risk_Id int,
    @Insurance_File_Cnt int,
    @Extra_Type_Id int,
    @Percent numeric(19,8),
    @Value numeric(19,4),
    @Is_Commissionable tinyint,
    @Is_Taxable tinyint,
    @Row_Added tinyint OUTPUT
AS


BEGIN
/*************************************************************************/
/* PURPOSE:  Insert an Extra, based upon the INPUT values, into either   */
/*           Ins_File_Extra_Value if the Risk_Id < 1 or                  */
/*           Risk_Extra_Value otherwise.  If the extra being inserted    */
/*           already exists in either of these tables, the value in the  */
/*           table will be deleted and the new one inserted.             */
/*                                                                       */
/* DOCUMENTS:\\arp\c\windows\personal\Extras.doc                         */
/*                                                                       */
/* INPUTS:   @Risk_Id                                                    */
/*           @Insurance_File_Cnt                                         */
/*           @Extra_Type_Id                                              */
/*           @Percent                                                    */
/*           @Value                                                      */
/*           @Is_Commissionable                                          */
/*           @Is_Taxable                                                 */
/*                                                                       */
/* OUTPUTS:  @Row_Added                                                         */
/*                                                                       *//*************************************************************************/
/*************************************************************************/
/* Revision Description of Modification                    Date       Who*/
/* 1.0      Original                                       14/08/1997 PH */
/*************************************************************************/
    /* Work with Risk_Extra_Value or Ins_File_Extra_Value? */
    IF (@Risk_Id < 1)
        BEGIN
            /* Work with Ins_File_Extra_Value */
            DELETE Ins_File_Extra_Value
             WHERE Insurance_File_Cnt = @Insurance_File_Cnt
               AND Extra_Type_Id = @Extra_Type_Id
            INSERT INTO Ins_File_Extra_Value
                (
                Insurance_File_Cnt         ,
                Extra_Type_Id              ,
                [Percent]                  ,
                Value                      ,
                Is_Commissionable          ,
                Is_Taxable
                )
            VALUES
                (
                @Insurance_File_Cnt        ,
                @Extra_Type_Id             ,
                @Percent                   ,
                @Value                     ,
                @Is_Commissionable         ,
                @Is_Taxable
                )
        IF (@@ROWCOUNT = 0)
                    BEGIN
                        SELECT @Row_Added = 0
                    END
                ELSE
                    BEGIN
                        SELECT @Row_Added = 1
                    END
        END
    ELSE
        BEGIN
            /* Work with Risk_Extra_Value */

            DELETE Risk_Extra_Value
             WHERE Risk_cnt = @Risk_Id
           AND Extra_Type_Id = @Extra_Type_Id
            INSERT INTO Risk_Extra_Value
                (
                Risk_Cnt                   ,
                Extra_Type_Id              ,
                Percentage                 ,
                Value                      ,
                Is_Commissionable          ,
                Is_Taxable
                )
            VALUES
                (
                @Risk_Id                   ,
                @Extra_Type_Id             ,
                @Percent                   ,
                @Value                     ,
                @Is_Commissionable         ,
                @Is_Taxable
                )
        IF (@@ROWCOUNT = 0)
                    BEGIN
                        SELECT @Row_Added = 0
                    END
                ELSE
                    BEGIN
                        SELECT @Row_Added = 1
                    END
        END
END
GO


