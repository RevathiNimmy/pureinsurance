SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Setup_Trans_Mapping_RSA'
GO


CREATE PROCEDURE spu_Setup_Trans_Mapping_RSA
AS


/****************************************************************************************************/
/* 'spu_Setup_Trans_Mapping_RSA' oversees the adding of required records into the                    */
/* Export_Map_Detail & Export_Map_Folder tables.                                                    */
/*                                                                                                  */
/****************************************************************************************************/
/* Revision     Description of Modification                                 Date        Who         */
/* --------     ---------------------------                                 ----        ---         */
/* 1.0          Original                                                    02/05/2001  RWH         */
/*                                                                                                  */
/* 1.1          Add in EXPTAX for taxes not applied to customer.            10/07/2001  RWH         */
/* 1.2          Added Claims stuff.                                         18/07/2001  RWH         */
/* 1.3          Map other party types.                                      23/07/2001  RWH         */
/****************************************************************************************************/

/* Format of calls to sub-query.

spu_Setup_Export_Map
    @new_target_field_name varchar(40),
    @new_acc_type_leading_characters varchar(255),
    @new_mapping_leading_characters varchar(255)
*/

-- Income \ Gross Written Premium
EXEC spu_Setup_Export_Map
    'INCGWP',
    '1 {INCOME}',
    'Gross Written Premium'

-- Current Assets \ Sales Ledger \ Other Party Receivable
EXEC spu_Setup_Export_Map
    'OTRECLEDGR',
    '3 {ASSETS}',
    "Other Party R''able"

-- Current Liabilities \ O/S Claims Adjustments
EXEC spu_Setup_Export_Map
    'OTPAYLEDGR',
    '4 {LIABILITIES}',
    'Other Party Payable'

-- Current Liabilities \ Tax
EXEC spu_Setup_Export_Map
    'LIABTAX',
    '4 {LIABILITIES}',
    'Tax'

-- Expenses \ Lead Commission
EXEC spu_Setup_Export_Map
    'EXPCOM',
    '2 {EXPENSES}',
    'Lead Commission'

-- Expenses \ RI Treaty Premium
EXEC spu_Setup_Export_Map
    'EXPRIOUTTR',
    '2 {EXPENSES}',
    'RI Treaty Premium'

-- Expenses \ RI Other Premium
EXEC spu_Setup_Export_Map
    'EXPRIOUTOT',
    '2 {EXPENSES}',
    'RI Other Premium'

-- Income \ RI Treaty Commission
EXEC spu_Setup_Export_Map
    'INCRICOMTR',
    '1 {INCOME}',
    'RI Treaty Commission'

-- Income \ RI Other Commission
EXEC spu_Setup_Export_Map
    'INCRICOMOT',
    '1 {INCOME}',
    'RI Other Commission'

-- Expenses \ CI Premium
EXEC spu_Setup_Export_Map
    'EXPCIOUT',
    '2 {EXPENSES}',
    'Coinsured Premium'

-- Income \ CI Commission
EXEC spu_Setup_Export_Map
    'INCCICOM',
    '1 {INCOME}',
    'Coinsured Commission'

-- Expense \ Tax Expense
EXEC spu_Setup_Export_Map
    'EXPTAX',
    '2 {EXPENSES}',
    'Tax Expense'

-- *****************************  Claims  *********************************

-- ******************
-- ***** Assets *****
-- ******************
-- Current Assets \ Sales Ledger \ Claims Receivable
EXEC spu_Setup_Export_Map
    'CLMREC',
    '3 {ASSETS}',
    'Claims Receivable'

-- ***********************
-- ***** Liabilities *****
-- ***********************
-- Current Liabilities \ Claims Payable
EXEC spu_Setup_Export_Map
    'CLMPAY',
    '4 {LIABILITIES}',
    'Claims Payable'

-- Current Liabilities \ O/S Claims Adjustments
EXEC spu_Setup_Export_Map
    'CLMRES',
    '4 {LIABILITIES}',
    'O/S Claims Adj'

-- Current Liabilities \ O/S Claims Adjustments
EXEC spu_Setup_Export_Map
    'CLMSUS',
    '4 {LIABILITIES}',
    'O/S Claims Adj'

-- ******************
-- ***** Income *****
-- ******************
-- Income \ RI Treaty Claims Recovery
EXEC spu_Setup_Export_Map
    'CLMRITR',
    '1 {INCOME}',
    'RI TTY Claims Rec.'

-- Income \ RI Other Claims Recovery
EXEC spu_Setup_Export_Map
    'CLMRIOT',
    '1 {INCOME}',
    'RI Other Claims Rec.'

-- Income \ CI Claims Recovery
EXEC spu_Setup_Export_Map
    'CLMCI',
    '1 {INCOME}',
    'CI Claims Rec.'

-- Income \ Salvage Recovery
EXEC spu_Setup_Export_Map
    'CLMSAL',
    '1 {INCOME}',
    'Claims Salvage Rec.'

-- Income \ TP Recovery
EXEC spu_Setup_Export_Map
    'CLMTPR',
    '1 {INCOME}',
    'Claims TP Rec.'

-- *******************
-- ***** Expense *****
-- *******************
-- Expense \ Gross Claims Incurred
EXEC spu_Setup_Export_Map
    'CLMEXP',
    '2 {EXPENSES}',
    'Grs Claims Incurred'

-- Expense \ RI Treaty Salvage Recovery
EXEC spu_Setup_Export_Map
    'CLMRITRSAL',
    '2 {EXPENSES}',
    'RI TTY Salvage Rec.'

-- Expense \ RI Other Salvage Recovery
EXEC spu_Setup_Export_Map
    'CLMRIOTSAL',
    '2 {EXPENSES}',
    'RI Other Salvage Rec'

-- Expense \ CI Salvage Recovery
EXEC spu_Setup_Export_Map
    'CLMCISAL',
    '2 {EXPENSES}',
    'CI Salvage Rec.'

-- Expense \ RI Treaty TP Recovery
EXEC spu_Setup_Export_Map
    'CLMRITRTPR',
    '2 {EXPENSES}',
    'RI TTY TP Rec.'

-- Expense \ RI Other TP Recovery
EXEC spu_Setup_Export_Map
    'CLMRIOTTPR',
    '2 {EXPENSES}',
    'RI Other TP Rec.'

-- Expense \ CI TP Recovery
EXEC spu_Setup_Export_Map
    'CLMCITPR',
    '2 {EXPENSES}',
    'CI TP Rec.'
GO


