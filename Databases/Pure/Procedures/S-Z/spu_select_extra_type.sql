SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_select_extra_type'
GO


CREATE PROCEDURE spu_select_extra_type
    @risk_id int,
    @trans_type_basis char(1),
    @effective_date datetime
AS


BEGIN
    /****************************************************************************/
    /* PURPOSE:  SELECTS the extra type and its associated Extra data from the  */
    /*           extra_type table.                                              */
    /*                                                                          */
    /* DOCUMENTS:\\arp\c\windows\personal\Extras.doc                            */    /*                                                                          */
    /* INPUTS:   @risk_id                                                       */
    /*           @trans_type_basis                                              */
    /*           @effective_date                                                */
    /*                                                                          */
    /* OUTPUTS:..the following rows using a SELECT statement:-                  */
    /*           description                                                    */
    /*           calculation_method                                             */
    /*           percent                                                        */
    /*           value                                                          */
    /*           is_taxable                                                     */
    /*           is_commissionable                                              */
    /*           code                                                           */
    /*           extra_type_id                                                  */
    /*                                                                          */
    /****************************************************************************/
    /****************************************************************************/
    /* Revision Description of Modification                    Date       Who   */
    /* 1.0      Original                                       14/08/1997 PH    */
    /****************************************************************************/
    /*
      Create list of all extra_type_ids
    */
    SELECT extra_type_id
      INTO #Temp_Extra_Type      FROM Extra_Type
    /*
      Delete ids from list where 'apply_to' values = 'N'
    */
    -- Primary or Primary Reversal
    IF (@trans_type_basis = 'P') OR (@trans_type_basis = 'R')
    BEGIN
        DELETE #Temp_Extra_Type
              FROM #Temp_Extra_Type T,
                   Extra_Type E
             WHERE E.apply_to_primary = 'N'
               AND E.extra_type_id = T.extra_type_id
    END
    -- Additions
    IF (@trans_type_basis = 'A')
    BEGIN
        DELETE #Temp_Extra_Type
              FROM #Temp_Extra_Type T,
                   Extra_Type E
             WHERE E.apply_to_addition = 'N'
               AND E.extra_type_id = T.extra_type_id
    END
    -- Refunds
    IF (@trans_type_basis = 'F')
    BEGIN
        DELETE #Temp_Extra_Type
              FROM #Temp_Extra_Type T,
                   Extra_Type E
             WHERE E.apply_to_refund = 'N'
               AND E.extra_type_id = T.extra_type_id
    END
    /*
       Choose whether to process Risk_Extra_Value
       or Ins_File_Extra_Value
    */
    IF @Risk_id < 1
        BEGIN
            /*
               Narrow list down to ids that don't exist
               in Ins_File_Extra_Value
            */
            DELETE #Temp_Extra_Type
              FROM #Temp_Extra_Type T,
                   Ins_File_Extra_Value I
             WHERE I.extra_type_id = T.extra_type_id
    END
    ELSE
        BEGIN
            /*
               Narrow list down to ids that don't exist
               in Risk_Extra_Value
            */
            DELETE #Temp_Extra_Type
              FROM #Temp_Extra_Type T,
                   Risk_Extra_Value R
             WHERE R.extra_type_id = T.extra_type_id
    END
        /*
           Select code and associated data from
           the final list
        */
        SELECT DISTINCT
               description              ,
               calculation_method   ,
               [percent]            ,
               value                ,
               is_taxable       ,
               is_commissionable        ,
               code         ,
           E.extra_type_id
          FROM Extra_Type E,
               #Temp_Extra_Type T
         WHERE E.extra_type_id = T.extra_type_id
           AND E.is_deleted = 0
           AND effective_date <= @effective_date
END
GO


